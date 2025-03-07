using AutoMapper;
using static VaccineScheduleTracking.API_Test.Helpers.ValidationHelper;
using static VaccineScheduleTracking.API_Test.Helpers.TimeSlotHelper;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Appointments;
using VaccineScheduleTracking.API_Test.Repository;
using VaccineScheduleTracking.API_Test.Repository.Appointments;
using VaccineScheduleTracking.API_Test.Repository.Children;
using VaccineScheduleTracking.API_Test.Repository.DailyTimeSlots;
using VaccineScheduleTracking.API_Test.Repository.Vaccines;
using VaccineScheduleTracking.API_Test.Services.Appointments;
using VaccineScheduleTracking.API_Test.Services.Children;
using VaccineScheduleTracking.API_Test.Services.DailyTimeSlots;
using VaccineScheduleTracking.API_Test.Services.Vaccines;
using static System.Runtime.InteropServices.JavaScript.JSType;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API_Test.Services.Doctors;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VaccineScheduleTracking.API_Test.Services.Record;
using VaccineScheduleTracking.API_Test.Models.DTOs;
using System.Net.WebSockets;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Reflection.PortableExecutable;
using VaccineScheduleTracking.API_Test.Helpers;

namespace VaccineScheduleTracking.API_Test.Services.Appointments
{
    public class AppointmentService : IAppointmentService
    {
        private readonly TimeSlotHelper _timeSlotHelper;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IVaccineRepository _vaccineRepository;
        private readonly IDailyScheduleRepository _dailyScheduleRepository;

        private readonly IVaccineRecordService _vaccineRecordService;
        private readonly IChildService _childServices;
        private readonly IDoctorServices _doctorServices;
        private readonly ITimeSlotServices _timeSlotServices;
        private readonly IVaccineService _vaccineServices;

        private readonly IMapper _mapper;

        public AppointmentService(
            TimeSlotHelper timeSlotHelper,
            IAppointmentRepository appointmentRepository,
            IVaccineRepository vaccineRepository,
            IDailyScheduleRepository dailyScheduleRepository,

            IVaccineRecordService vaccineRecordService,
            IChildService childServices,
            IDoctorServices doctorServices,
            ITimeSlotServices timeSlotServices,
            IVaccineService vaccineServices,

            IMapper mapper)
        {
            _timeSlotHelper = timeSlotHelper;
            _appointmentRepository = appointmentRepository;
            _dailyScheduleRepository = dailyScheduleRepository;
            _vaccineRepository = vaccineRepository;

            _vaccineRecordService = vaccineRecordService;
            _childServices = childServices;
            _doctorServices = doctorServices;
            _timeSlotServices = timeSlotServices;
            _vaccineServices = vaccineServices;

            _mapper = mapper;
        }

        public async Task<Appointment?> GetAppointmentByIDAsync(int appointmentID)
        {
            ValidateInput(appointmentID, "chưa nhập ID");
            return await _appointmentRepository.GetAppointmentByIDAsync(appointmentID);
        }


        public async Task<List<Appointment>> GetAppointmentByDateAsync(int childId, DateOnly date)
        {
            ValidateInput(childId, "chưa nhập child ID");
            ValidateInput(date, "chưa nhập ngày");

            return await _appointmentRepository.GetAppointmentByDateAsync(childId, date);
        }

        /// <summary>
        /// giới hạn số lượng appointment cho 1 ngày
        /// </summary>
        /// <param name="childId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task LimitAmount(int childId, DateOnly date)
        {
            var appointments = await GetAppointmentByDateAsync(childId, date);
            var validAppointments = appointments
                .Where(a => a.Status == "FINISHED" || a.Status == "PENDING")
                .ToList();
            if (validAppointments.Count >= 5)
            {
                throw new Exception("không thể đăng kí quá 4 lịch tiêm cho 1 ngày");
            }
        }
        public async Task<List<Appointment>> GetPendingDoctorAppointmentAsync(int doctorId)
        {
            return await _appointmentRepository.GetPendingDoctorAppointmentAsync(doctorId);
        }


        /// <summary>
        /// tìm ngày tiêm vaccine x gần nhất 
        /// </summary>
        /// <param name="childId"></param>
        /// <param name="vaccineId"></param>
        /// <param name="appointmentDate"></param>
        /// <returns></returns>
        public async Task<DateOnly?> GetLatestVaccineDate(int childId, int vaccineId)
        {
            var appointments = await GetChildAppointmentsAsync(childId);
            var validAppointments = appointments
               .Where(a => a.Status == "FINISHED" || a.Status == "PENDING")
               .ToList();
            var vacHistory = await _vaccineRecordService.GetRecordsAsync(childId);

            DateOnly? latestDate = DateOnly.MinValue;
            foreach (var appointment in validAppointments)
            {
                if (appointment.VaccineID == vaccineId)
                {
                    if (appointment.TimeSlots.DailySchedule.AppointmentDate > latestDate)
                    {
                        latestDate = appointment.TimeSlots.DailySchedule.AppointmentDate;
                    }
                }
            }
            foreach (var record in vacHistory)
            {
                if (record.VaccineID == vaccineId)
                {
                    if (record.Date > latestDate)
                    {
                        latestDate = record.Date;
                    }
                }
            }
            return latestDate;
        }


        /// <summary>
        /// giới hạn thời gian cùng loại vaccine
        /// </summary>
        /// <param name="vaccineId"></param>
        /// <param name="childId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<(bool canSchedule, DateOnly? limitDate)> LimitVaccinePeriod(int vaccineId, int childId, DateOnly date)
        {
            var vaccine = await _vaccineServices.GetVaccineByIDAsync(vaccineId);
            DateOnly? latestDate = await GetLatestVaccineDate(childId, vaccineId);

            if (latestDate == null) return (true, null);

            DateOnly limitDate = _timeSlotHelper.GetPeriodDate(vaccine.Period, latestDate.Value);

            return (date > limitDate, limitDate);
        }



        /// <summary>
        /// Chuyển trạng thái Appointment - đã xong
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Appointment?> SetAppointmentStatusAsync(int appointmentId, string status, string? note)
        {
            //ValidateInput(appointmentId, "Id buổi hẹn không thể để trống");
            string s = ValidateStatus(status);
            var appointment = await _appointmentRepository.GetAppointmentByIDAsync(appointmentId);
            if (appointment == null)
                throw new Exception("không tìm thấy buổi hẹn");
            else if (appointment.Status == "OVERDUE")
                throw new Exception(" buổi hẹn đã quá hạn");
            else if (appointment.Status == "CANCELED")
                throw new Exception(" buổi hẹn đã bị hủy");

            appointment.Status = s;

            if (s.Equals("FINISHED"))
            {
                await AddAppointmentToRecord(appointment, note);
            }
            return await _appointmentRepository.UpdateAppointmentAsync(appointment);
        }


        public async Task AddAppointmentToRecord(Appointment appointment, string? note)
        {
            ValidateInput(appointment, "appointment đưa vào chưa đầy đủ thông tin");
            var record = new CreateVaccineRecordDto
            {
                ChildID = appointment.ChildID,
                AppointmentID = appointment.AppointmentID,
                VaccineTypeID = appointment.Vaccine.VaccineTypeID,
                VaccineID = appointment.VaccineID,
                Date = appointment.TimeSlots.DailySchedule.AppointmentDate,
                Note = note ?? "autoMsg: không phản ứng phụ được ghi nhận"
            };

            var rec = await _vaccineRecordService.AddVaccineRecordAsync(record).ConfigureAwait(false);
            if (rec is null)
            {
                throw new InvalidOperationException($"Không thể tạo vaccineRecord cho Appointment {appointment.AppointmentID}");
            }

        }


        /// <summary>
        /// hủy Appointment - đã xong
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Appointment?> CancelAppointmentAsync(int appointmentId)
        {
            ValidateInput(appointmentId, "Id buổi hẹn không thể để trống");
            var appointment = await _appointmentRepository.GetAppointmentByIDAsync(appointmentId);

            DateOnly date = appointment.TimeSlots.DailySchedule.AppointmentDate;
            int slotNumber = appointment.TimeSlots.SlotNumber;
            int doctorID = appointment.Account.Doctor.DoctorID;
            int childID = appointment.ChildID;
            string doctorName = $"{appointment.Account.Lastname} {appointment.Account.Firstname}";

            if (appointment == null)
            {
                throw new Exception($"không tìm thấy buổi hẹn có ID: {appointmentId}");
            }
            if (appointment.Status.ToUpper() == "OVERDUE")
            {
                throw new Exception($"buổi hẹn ngày {date} slot {slotNumber} đã quá hạn");
            }

            var childTimeSlot = await _childServices.GetChildTimeSlotBySlotNumberAsync(childID, slotNumber, date);
            if (childTimeSlot == null)
            {
                Console.WriteLine($"Dữ liệu bất thường:\nChildTimeSlot Ngày: {date} slot {slotNumber} không tồn tại");
            }
            else if (childTimeSlot.Available == false)
            {
                Console.WriteLine($"Dữ liệu bất thường:\nChildTimeSlot Ngày: {date} slot {slotNumber} đã được set thành false trước đó");
            }

            var doctorTimeSlot = await _doctorServices.FindDoctorTimeSlotAsync(doctorID, date, slotNumber);
            if (doctorTimeSlot.Available == null)
            {
                Console.WriteLine($"Dữ liệu bất thường:\nDoctorTimeSLot Ngày: {date} slot {slotNumber} của bác sĩ {doctorName} không tồn tại (có lẽ là từ tạo doctortimeslot // tạo thiếu ngày)");
            }
            else if (doctorTimeSlot.Available == false)
            {
                Console.WriteLine($"Dữ liệu bất thường:\nDoctorTimeSLot Ngày: {date} slot {slotNumber} của bác sĩ {doctorName} đã bị hủy trước đó");
            }

            if (appointment.Status == "PENDING")
            {
                Console.WriteLine($"Dữ liệu bất thường:\ncuộc hẹn ngày {date} slot {slotNumber} đang chưa được đăng kí");
            }
            doctorTimeSlot.Available = true;
            childTimeSlot.Available = false;
            appointment.Vaccine.Stock += 1;
            appointment.Status = "CANCELED";

            return await _appointmentRepository.UpdateAppointmentAsync(appointment);
        }




        /// <summary>
        /// hàm tạo lịch hẹn - đang sửa 
        /// </summary>
        /// <param name="createAppointment"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Appointment?> CreateAppointmentAsync(CreateAppointmentDto createAppointment)
        {
            DateOnly date = createAppointment.Date;
            int slotNumber = createAppointment.SlotNumber;
            int childID = createAppointment.ChildID;
            int vaccineID = createAppointment.VaccineID;

            try
            {
                DateTime dt = date.ToDateTime(new TimeOnly(0, 0));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new Exception("Format của date không đúng.");
            }

            // Catch boundary cho ngày đặt lịch 
            _timeSlotHelper.LimitDate(date, "Hiện tại chỉ có thể đặt lịch trước ngày");

            //giới hạn mũi tiêm
            await LimitAmount(childID, date);
            var (Available, limitDate) = await LimitVaccinePeriod(vaccineID, childID, date);
            if (!Available)
            {
                throw new Exception($"Vaccine này đã được dùng gần đây, sẽ khả dụng sau ngày {limitDate}");
            }


            var timeSlot = await _timeSlotServices.GetTimeSlotAsync(slotNumber, date);
            ValidateInput(timeSlot, "Slot nhập vào không hợp lệ (1 - 20)");
            if (!timeSlot.Available)
            {
                throw new Exception("Slot này đã quá hạn.");
            }

            // Kiểm tra trẻ có tồn tại không
            var child = await _childServices.GetChildByIDAsync(childID);
            if (child == null)
            {
                throw new Exception($"Không tìm thấy trẻ có ID {childID}");
            }

            // Kiểm tra trẻ đã đặt slot này chưa 
            var childTimeSlot = await _childServices.GetChildTimeSlotBySlotNumberAsync(childID, slotNumber, date);
            if (childTimeSlot != null && childTimeSlot.Available == false)
            {
                throw new Exception("Slot này đã được đăng ký");
            }

            var vaccine = await _vaccineServices.GetVaccineByIDAsync(vaccineID);
            if (vaccine == null)
            {
                throw new Exception($"Không tìm thấy vaccine có ID: {vaccineID}");
            }

            // Kiểm tra bác sĩ còn slot không
            var doctorSlot = await _doctorServices.GetSuitableDoctorTimeSlotAsync(slotNumber, date);
            if (doctorSlot == null)
            {
                throw new Exception($"Đã hết bác sĩ ở slot {slotNumber}");
            }
            var docAccount = await _doctorServices.GetDoctorByIDAsync(doctorSlot.DoctorID);

            var appointment = new Appointment
            {
                ChildID = childID,
                AccountID = docAccount.AccountID,
                VaccineID = vaccineID,
                TimeSlotID = timeSlot.TimeSlotID,
                Status = "PENDING"
            };

            // Cập nhật TimeSlot cho trẻ - tách hàm 
            if (childTimeSlot != null && childTimeSlot.Available == true)
            {
                childTimeSlot.Available = false;
                await _childServices.UpdateChildTimeSlotAsync(childTimeSlot);
            }
            else
            {
                await _childServices.CreateChildTimeSlot(slotNumber, date, childID);
            }

            // cập nhật DoctorTimeSlot
            await _doctorServices.SetDoctorTimeSlotAsync(doctorSlot, false);

            // cập nhật số lượng vaccine - Tách hàm
            vaccine.Stock -= 1;
            await _vaccineRepository.UpdateVaccineAsync(vaccine);

            return await _appointmentRepository.CreateAppointmentAsync(appointment);
        }



        /// <summary>
        /// cập nhật lịch hẹn (Appointment)
        /// </summary>
        /// <param name="appointmentID"></param>
        /// <param name="modAppointment"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Appointment?> UpdateAppointmentAsync(int appointmentID, UpdateAppointmentDto modAppointment)// đã xong - chưa tối ưu
        {
            var appointment = await _appointmentRepository.GetAppointmentByIDAsync(appointmentID);
            int oldChildId = appointment.ChildID;
            int oldDoctorId = appointment.Account.Doctor.DoctorID;
            int oldVaccineId = appointment.VaccineID;
            int oldSlotNumber = appointment.TimeSlots.SlotNumber;
            DateOnly oldDate = appointment.TimeSlots.DailySchedule.AppointmentDate;

            ValidateInput(appointment, "Lịch hẹn không tồn tại!");
            _timeSlotHelper.LimitDate(modAppointment.Date, "TimeSlot chỉ được tạo đến trước ngày");

            if (appointment.TimeSlots.Available == false)
            {
                throw new Exception("lịch hẹn này đã quá hạn, vui lòng đăng kí lịch hẹn mới");
            }

            var tempAppoint = new UpdateAppointmentDto();
            int upChildId = tempAppoint.ChildID;
            int upDoctorId = tempAppoint.DoctorID;
            int upVaccineId = tempAppoint.VaccineID;
            int upSlotNumber = tempAppoint.SlotNumber;
            DateOnly upDate = tempAppoint.Date;

            upChildId = NullValidator(modAppointment.ChildID)
                ? modAppointment.ChildID
                : oldChildId;
            upDoctorId = NullValidator(modAppointment.DoctorID)
                ? modAppointment.DoctorID
                : oldDoctorId;
            upVaccineId = NullValidator(modAppointment.VaccineID)
                ? modAppointment.VaccineID
                : oldVaccineId;
            upSlotNumber = NullValidator(modAppointment.SlotNumber)
              ? modAppointment.SlotNumber
              : oldSlotNumber;
            upDate = NullValidator(modAppointment.Date)
                ? modAppointment.Date
                : oldDate;

            ///kiểm tra ngày hẹn
            if (_timeSlotHelper.CompareNowTime(oldDate) == -1)
            {
                throw new Exception("không thể đặt ngày đã qua hạn!");
            }

            ///giới hạn tiêm 
            LimitAmount(upChildId, upDate);
            var (Available, limitDate) = await LimitVaccinePeriod(upVaccineId, upChildId, upDate);
            if (!Available)
            {
                throw new Exception($"Vaccine này đã được dùng gần đây và sẽ khả dụng sau ngày {limitDate}");
            }

            ///kiểm tra slot
            var timeSlot = await _timeSlotServices.GetTimeSlotAsync(upSlotNumber, upDate);
            if (timeSlot.Available == false)
            {
                throw new Exception("không thể đặt slot đã quá hạn!");
            }

            ///kiểm DoctorSlot
            var doctorTimeSlot = await _doctorServices.FindDoctorTimeSlotAsync(upDoctorId, upDate, upSlotNumber);
            if (doctorTimeSlot == null || !doctorTimeSlot.Available)
            {
                var docAccount = await _doctorServices.GetDoctorByIDAsync(upDoctorId);
                throw new Exception($"Bác sĩ {docAccount.Firstname} không còn lịch trống vào ngày {upDate} slot {upSlotNumber} Chọn bác sĩ khác hoặc đổi thời gian hẹn lịch nhé <3");
            }

            ///kiểm tra slot này đã được đăng kí chưa
            var childTimeSlot = await _childServices.GetChildTimeSlotBySlotNumberAsync(upChildId, upSlotNumber, upDate);
            if (childTimeSlot != null && childTimeSlot.Available)
            {
                throw new Exception("Bạn đã đăng kí Slot này");
            }

            ///kiểm tra vaccine
            var vaccine = await _vaccineServices.GetVaccineByIDAsync(upVaccineId);
            if (vaccine.Stock == 0)
            {
                throw new Exception($"Loại vaccine {vaccine.Name} này đã hết");
            }

            //------------------các hàm cập nhật lại-------------------- 
            ///cập nhật đổi doctor
            var oldDoctorSlot = await _doctorServices.FindDoctorTimeSlotAsync(oldDoctorId, oldDate, oldSlotNumber);
            if (modAppointment.DoctorID != 0 && modAppointment.DoctorID != oldDoctorId)
            {
                if (oldDoctorSlot != null)
                {
                    oldDoctorSlot.Available = true;
                }
                doctorTimeSlot.Available = false;

                oldDoctorId = doctorTimeSlot.DoctorID;
            }
            ///cập nhật lại stock vaccine
            if (modAppointment.VaccineID != 0 && modAppointment.VaccineID != oldVaccineId)
            {
                var oldVaccine = await _vaccineServices.GetVaccineByIDAsync(oldVaccineId);
                if (oldVaccine != null)
                {
                    oldVaccine.Stock += 1;
                }
                vaccine.Stock -= 1;

                oldVaccineId = modAppointment.VaccineID;
            }

            ///cập nhật lại slot cho child + doctor - đang lỗi 
            if (modAppointment.SlotNumber != 0 && modAppointment.SlotNumber != oldSlotNumber
                || modAppointment.Date != default && modAppointment.Date != oldDate)
            {
                var oldChildSlot = await _childServices.GetChildTimeSlotBySlotNumberAsync(oldChildId, oldSlotNumber, oldDate);
                if (oldChildSlot != null)
                {
                    oldChildSlot.Available = false;
                }
                if (childTimeSlot != null && childTimeSlot.Available == false)
                {
                    childTimeSlot.Available = true;
                }
                else
                {
                    await _childServices.CreateChildTimeSlot(upSlotNumber, upDate, upChildId);
                }
                if (oldDoctorSlot != null)
                {
                    oldDoctorSlot.Available = true;
                }
                doctorTimeSlot.Available = false;

                appointment.VaccineID = upVaccineId;
                appointment.Account.Doctor.DoctorID = doctorTimeSlot.DoctorID;
                appointment.TimeSlotID = timeSlot.TimeSlotID;
            }

            return await _appointmentRepository.UpdateAppointmentAsync(appointment);
        }




        /// <summary>
        /// hàm lấy danh sách lịch hẹn theo của Child
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Appointment>> GetChildAppointmentsAsync(int childId)
        {
            var child = await _childServices.GetChildByIDAsync(childId);
            var appointmentList = await _appointmentRepository.GetAppointmentsByChildIDAsync(childId);
            if (appointmentList.IsNullOrEmpty())
            {
                throw new Exception($"không tìm thấy buổi hẹn nào cho bé {child.Lastname} {child.Firstname}");
            }

            return appointmentList;
        }


        /// <summary>
        /// hàm lấy danh sách lịch hẹn theo doctor
        /// </summary>
        /// <param name="doctorId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Appointment>> GetDoctorAppointmentsAsync(int doctorId)
        {
            var docAccount = await _doctorServices.GetDoctorByIDAsync(doctorId);
            var doctorAppointments = await _appointmentRepository.GetAppointmentsByDoctorIDAsync(doctorId);
            if (doctorAppointments.IsNullOrEmpty())
            {
                throw new Exception($"không tìm thấy buổi hẹn nào cho bác sĩ {docAccount.Lastname} {docAccount.Firstname}");
            }
            return doctorAppointments;
        }



        /// <summary>
        /// tự động chuyển các apointment thành OVERDUE khi bị quá hạn
        /// </summary>
        /// <returns></returns>
        public async Task SetOverdueAppointmentAsync()// đã xong - chưa tối ưu
        {
            var dailySchedules = await _dailyScheduleRepository.GetAllDailyScheduleAsync();
            var allAppointments = await _appointmentRepository.GetAllAppointmentsAsync();

            foreach (var date in dailySchedules)
            {
                int dateStatus = _timeSlotHelper.CompareNowTime(date.AppointmentDate);
                var filteredAppointments = allAppointments.Where(a => a.TimeSlots.DailySchedule.AppointmentDate == date.AppointmentDate).ToList();

                if (dateStatus == -1) // Ngày đã qua
                {
                    foreach (var appointment in filteredAppointments)
                    {
                        if (appointment.Status == "PENDING")
                        {
                            appointment.Status = "OVERDUE";
                            await _appointmentRepository.UpdateAppointmentAsync(appointment);
                        }
                    }
                }
                else if (dateStatus == 0) // Ngày hôm nay, kiểm tra giờ
                {
                    foreach (var appointment in filteredAppointments)
                    {
                        if (_timeSlotHelper.CompareNowTime(appointment.TimeSlots.StartTime) == -1 && appointment.Status == "PENDING") // Giờ đã qua
                        {
                            appointment.Status = "OVERDUE";
                            await _appointmentRepository.UpdateAppointmentAsync(appointment);
                        }
                    }
                }
            }

        }





    }
}
