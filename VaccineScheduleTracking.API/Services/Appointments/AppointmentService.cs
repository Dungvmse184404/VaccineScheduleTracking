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

namespace VaccineScheduleTracking.API_Test.Services.Appointments
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IVaccineRepository _vaccineRepository;
        private readonly IDailyScheduleRepository _dailyScheduleRepository;

        private readonly IChildService _childServices;
        private readonly IDoctorServices _doctorServices;
        private readonly ITimeSlotServices _timeSlotServices;
        private readonly IVaccineService _vaccineServices;

        private readonly IMapper _mapper;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IVaccineRepository vaccineRepository,
            IDailyScheduleRepository dailyScheduleRepository,

            IChildService childServices,
            IDoctorServices doctorServices,
            ITimeSlotServices timeSlotServices,
            IVaccineService vaccineServices,

            IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _dailyScheduleRepository = dailyScheduleRepository;
            _vaccineRepository = vaccineRepository;

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


        public async Task<List<Appointment>> GetPendingDoctorAppointmentAsync(int doctorId)
        {
            return await _appointmentRepository.GetPendingDoctorAppointmentAsync(doctorId);
        }


        /// <summary>
        /// Chuyển trạng thái Appointment 
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Appointment?> SetAppointmentStatusAsync(int appointmentId, string status)
        {
            ValidateInput(appointmentId, "Id buổi hẹn không thể để trống");
            string s = ValidateStatus(status);
            var appointment = await _appointmentRepository.GetAppointmentByIDAsync(appointmentId);
            if (appointment == null) 
                throw new Exception("không tìm thấy buổi hẹn");
            else if (appointment.Status == "OVERDUE")
                throw new Exception(" buổi hẹn đã quá hạn");
            else if (appointment.Status == "CANCELED")
                throw new Exception(" buổi hẹn đã bị hủy");

            appointment.Status = s;

            return await _appointmentRepository.UpdateAppointmentAsync(appointment);
        }

        
        /// <summary>
        /// hủy Appointment
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Appointment?> CancelAppointmentAsync(int appointmentId)
        {
            ValidateInput(appointmentId, "Id buổi hẹn không thể để trống");
            var appointment = await _appointmentRepository.GetAppointmentByIDAsync(appointmentId);
            if (appointment == null)
            {
                throw new Exception($"không tìm thấy buổi hẹn có ID: {appointmentId}");
            }
            if (appointment.Status.ToUpper() == "OVERDUE")
            {
                throw new Exception($"buổi hẹn ngày {appointment.TimeSlots.DailySchedule.AppointmentDate} slot {appointment.TimeSlots.SlotNumber} đã quá hạn");
            }
            

            var childTimeSlot = await _childServices.GetChildTimeSlotBySlotNumberAsync(appointment.ChildID, appointment.TimeSlots.SlotNumber, appointment.TimeSlots.DailySchedule.AppointmentDate);
            if (childTimeSlot == null)
            {
                Console.WriteLine($"Dữ liệu bất thường:\nChildTimeSlot Ngày: {appointment.TimeSlots.DailySchedule.AppointmentDate} slot {appointment.TimeSlots.SlotNumber} không tồn tại");
            }
            else if (childTimeSlot.Available == false)
            {
                Console.WriteLine($"Dữ liệu bất thường:\nChildTimeSlot Ngày: {appointment.TimeSlots.DailySchedule.AppointmentDate} slot {appointment.TimeSlots.SlotNumber} đã được set thành false trước đó");
            }

            var doctorTimeSlot = await _doctorServices.FindDoctorTimeSlotAsync(appointment.DoctorID, appointment.TimeSlots.DailySchedule.AppointmentDate, appointment.TimeSlots.SlotNumber);
            if (doctorTimeSlot.Available == null)
            {
                Console.WriteLine($"Dữ liệu bất thường:\nDoctorTimeSLot Ngày: {appointment.TimeSlots.DailySchedule.AppointmentDate} slot {appointment.TimeSlots.SlotNumber} của doctor {appointment.Doctor.Account.Lastname} {appointment.Doctor.Account.Firstname} không tồn tại (có lẽ là từ tạo doctortimeslot // tạo thiếu ngày)");
            }
            else if (doctorTimeSlot.Available == false)
            {
                Console.WriteLine($"Dữ liệu bất thường:\nDoctorTimeSLot Ngày: {appointment.TimeSlots.DailySchedule.AppointmentDate} slot {appointment.TimeSlots.SlotNumber} của doctor {appointment.Doctor.Account.Lastname} {appointment.Doctor.Account.Firstname} đã bị hủy trước đó");
            } 


            if (appointment.Status == "PENDING")
            {
                Console.WriteLine($"Dữ liệu bất thường:\ncuộc hẹn ngày {appointment.TimeSlots.DailySchedule.AppointmentDate} slot {appointment.TimeSlots.SlotNumber} đang chưa được đăng kí");
            }
            doctorTimeSlot.Available = true;
            childTimeSlot.Available = false;
            appointment.Vaccine.Stock += 1;
            appointment.Status = "CANCELED"; 

            return await _appointmentRepository.UpdateAppointmentAsync(appointment);
        }



        /// <summary>
        /// hàm tạo lịch hẹn
        /// </summary>
        /// <param name="createAppointment"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Appointment?> CreateAppointmentAsync(CreateAppointmentDto createAppointment)
        {
            try
            {
                // Validate the date
                DateTime dt = createAppointment.Date.ToDateTime(new TimeOnly(0, 0));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new Exception("Format của date không đúng.");
            }

            ///catch lỗi đặt lịch vào ngày chưa được tạo TimeSlot
            LimitDate(createAppointment.Date, $"hiện tại chỉ có thể đặt lịch trước ngày ");

            ///check trạng thái của slot - đã xong
            var timeSlot = await _timeSlotServices.GetTimeSlotAsync(createAppointment.SlotNumber, createAppointment.Date);
            ValidateInput(timeSlot, "Slot nhập vào không hợp lệ (1 - 20)");
            if (timeSlot.Available == false)
            {
                throw new Exception("Slot này đã quá hạn.");
            }

            ///check trẻ có tồn tại không - đã xong
            var child = await _childServices.GetChildByIDAsync(createAppointment.ChildID);
            if (child == null)
            {
                throw new Exception($"Không tìm thấy trẻ có ID {createAppointment.ChildID}");
            }
            ///tạo ChildTimeSlot - đã xong
            var childTimeSlot = await _childServices.GetChildTimeSlotBySlotNumberAsync(createAppointment.ChildID, createAppointment.SlotNumber, createAppointment.Date);
            if (childTimeSlot != null)
            {
                throw new Exception($"Slot này đã được đăng kí");
            }

            ///kiểm tra vaccine 
            var vaccine = await _vaccineServices.GetVaccineByIDAsync(createAppointment.VaccineID);
            if (vaccine == null)
            {
                throw new Exception($"không tìm thấy vaccine có ID: {createAppointment.VaccineID}");
            }

            /// check bác sĩ trùng slot
            var doctorSlot = await _doctorServices.GetSuitableDoctorTimeSlotAsync(createAppointment.SlotNumber, createAppointment.Date);
            if (doctorSlot == null)
            {
                throw new Exception($"Đã hết bác sĩ ở slot {createAppointment.SlotNumber} ");
            }

            //-------------- trạng thái cho các field liên quan --------------
            var appointment = new Appointment();
            appointment.ChildID = child.ChildID;
            appointment.DoctorID = doctorSlot.DoctorID;
            appointment.VaccineID = createAppointment.VaccineID;
            appointment.TimeSlotID = timeSlot.TimeSlotID;
            appointment.Status = "PENDING";

            ///câp nhật timeslot cho trẻ 
            await _childServices.CreateChildTimeSlot(createAppointment.SlotNumber, createAppointment.Date, createAppointment.ChildID);

            ///cập nhật timeslot Doctor - gom lại thành func trong doctorServices
            var docSlot = await _doctorServices.SetDoctorTimeSlotAsync(doctorSlot, false);//mới sửa

            ///cập nhật lại số lượng vaccine - gon lại thành func trong vaccineServices
            vaccine.Stock -= 1;
            vaccine = await _vaccineRepository.UpdateVaccineAsync(vaccine);

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
            ValidateInput(appointment, "Lịch hẹn không tồn tại!");
            LimitDate(modAppointment.Date, "TimeSlot chỉ được tạo đến trước ngày");
            if (appointment.TimeSlots.Available == false)
            {
                throw new Exception("lịch hẹn này đã hết hạn, vui lòng đăng kí lịch hẹn mới");
            }

            var tempAppoint = new UpdateAppointmentDto();
            tempAppoint.ChildID = NullValidator(modAppointment.ChildID)
                ? modAppointment.ChildID
                : appointment.ChildID;
            tempAppoint.DoctorID = NullValidator(modAppointment.DoctorID)
                ? modAppointment.DoctorID
                : appointment.DoctorID;
            tempAppoint.VaccineID = NullValidator(modAppointment.VaccineID)
                ? modAppointment.VaccineID
                : appointment.VaccineID;
            tempAppoint.SlotNumber = NullValidator(modAppointment.SlotNumber)
              ? modAppointment.SlotNumber
              : appointment.TimeSlots.SlotNumber;
            tempAppoint.Date = NullValidator(modAppointment.Date)
                ? modAppointment.Date
                : appointment.TimeSlots.DailySchedule.AppointmentDate;

            ///kiểm tra ngày hẹn
            if (CompareNowTime(appointment.TimeSlots.DailySchedule.AppointmentDate) == -1)
            {
                throw new Exception("không thể đặt ngày đã qua hạn!");
            }

            ///kiểm tra slot
            var timeSlot = await _timeSlotServices.GetTimeSlotAsync(tempAppoint.SlotNumber, tempAppoint.Date);
            if (timeSlot.Available == false)
            {
                throw new Exception("không thể đặt slot đã quá hạn!");
            }

            ///kiểm DoctorSlot
            var doctorTimeSlot = await _doctorServices.FindDoctorTimeSlotAsync(tempAppoint.DoctorID, tempAppoint.Date, tempAppoint.SlotNumber);
            if (doctorTimeSlot.Available == false)
            {
                var doctor = await _doctorServices.GetDoctorByIDAsync(tempAppoint.DoctorID);
                throw new Exception($"Bác sĩ {doctor.Account.Firstname} không còn lịch trống vào ngày {appointment.TimeSlots.DailySchedule.AppointmentDate} slot {modAppointment.SlotNumber}\nChọn bác sĩ khác hoặc đổi thời gian hẹn lịch nhé <3");
            }

            ///kiểm tra slot này đã được đăng kí chưa
            var childTimeSlot = await _childServices.GetChildTimeSlotBySlotNumberAsync(tempAppoint.ChildID, tempAppoint.SlotNumber, tempAppoint.Date);
            if (childTimeSlot != null && childTimeSlot.Available == true)
            {
                throw new Exception("Bạn đã đăng kí Slot này");
            }

            ///kiểm tra vaccine
            var vaccine = await _vaccineServices.GetVaccineByIDAsync(tempAppoint.VaccineID);
            if (vaccine.Stock == 0)
            {
                throw new Exception($"Loại vaccine {vaccine.Name} này đã hết");
            }

            //------------------các hàm cập nhật lại-------------------- 
            ///cập nhật đổi doctor
            var oldDoctorSlot = await _doctorServices.FindDoctorTimeSlotAsync(appointment.DoctorID, appointment.TimeSlots.DailySchedule.AppointmentDate, appointment.TimeSlots.SlotNumber);
            if (modAppointment.DoctorID != 0 && modAppointment.DoctorID != appointment.DoctorID)
            {
                if (oldDoctorSlot != null)
                {
                    oldDoctorSlot.Available = true;
                }
                doctorTimeSlot.Available = false;

                appointment.DoctorID = doctorTimeSlot.DoctorID;
            }
            ///cập nhật lại stock vaccine
            if (modAppointment.VaccineID != 0 && modAppointment.VaccineID != appointment.VaccineID)
            {
                var oldVaccine = await _vaccineServices.GetVaccineByIDAsync(appointment.VaccineID);
                if (oldVaccine != null)
                {
                    oldVaccine.Stock += 1;
                }
                vaccine.Stock -= 1;

                appointment.VaccineID = modAppointment.VaccineID;
            }

            ///cập nhật lại slot cho child + doctor - đang lỗi 
            if (modAppointment.SlotNumber != 0 && modAppointment.SlotNumber != appointment.TimeSlots.SlotNumber
                || modAppointment.Date != default && modAppointment.Date != appointment.TimeSlots.DailySchedule.AppointmentDate)
            {
                var oldChildSlot = await _childServices.GetChildTimeSlotBySlotNumberAsync(appointment.ChildID, appointment.TimeSlots.SlotNumber, appointment.TimeSlots.DailySchedule.AppointmentDate);
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
                    await _childServices.CreateChildTimeSlot(tempAppoint.SlotNumber, tempAppoint.Date, appointment.ChildID);
                }
                if (oldDoctorSlot != null)
                {
                    oldDoctorSlot.Available = true;
                }
                doctorTimeSlot.Available = false;

                appointment.DoctorID = doctorTimeSlot.DoctorID;
                appointment.TimeSlotID = timeSlot.TimeSlotID;
            }

            return await _appointmentRepository.UpdateAppointmentAsync(appointment);
        }




        /// <summary>
        /// hàm lấy danh sách lịch hẹn theo ID và role
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Appointment>> GetAppointmentListByIDAsync(int id, string role)
        {
            ValidateInput(role, "Role không được để trống");
            ValidateInput(id, "ID can't be empty");

            List<Appointment> appointmentList;
            switch (role.ToLower().Trim())
            {
                case "child":
                    appointmentList = await _appointmentRepository.GetAppointmentListByChildIDAsync(id);
                    break;
                case "doctor":
                    appointmentList = await _appointmentRepository.GetAppointmentListByDoctorIDAsync(id);
                    break;
                default:
                    throw new Exception("Role không hợp lệ try:(child || doctor)");
            }
            return appointmentList;
        }




        public async Task SetOverdueAppointmentAsync()// đã xong - chưa tối ưu
        {
            var dailySchedules = await _dailyScheduleRepository.GetAllDailyScheduleAsync();
            var allAppointments = await _appointmentRepository.GetAllAppointmentsAsync();

            foreach (var date in dailySchedules)
            {
                int dateStatus = CompareNowTime(date.AppointmentDate);
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
                        if (CompareNowTime(appointment.TimeSlots.StartTime) == -1 && appointment.Status == "PENDING") // Giờ đã qua
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
