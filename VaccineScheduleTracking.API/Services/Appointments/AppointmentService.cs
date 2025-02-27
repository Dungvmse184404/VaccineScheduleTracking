using AutoMapper;
using static VaccineScheduleTracking.API_Test.Helpers.ValidationHelper;
using static VaccineScheduleTracking.API_Test.Helpers.TimeSlotHelper;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API.Services;
using VaccineScheduleTracking.API_Test.Helpers;
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

        /// <summary>
        /// hàm tạo lịch hẹn
        /// </summary>
        /// <param name="createAppointment"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Appointment?> CreateAppointmentAsync(CreateAppointmentDto createAppointment)
        {
            ///check trạng thái của slot - đã xong
            var timeSlot = await _timeSlotServices.GetTimeSlotAsync(createAppointment.SlotNumber, createAppointment.Date);
            if (timeSlot.Available == false)
            {
                throw new Exception("Slot này không khả dụng.");
            }

            ///check trẻ có tồn tại không - đã xong
            var child = await _childServices.GetChildByIDAsync(createAppointment.ChildID);
            if (child == null)
            {
                throw new Exception($"Không tìm thấy trẻ có ID {createAppointment.ChildID}");
            }
            ///tạo ChildTimeSlot - đã xong
            var childTimeSlot = await _childServices.GetChildTimeSlotBySlotNumberAsync(createAppointment.SlotNumber, createAppointment.Date);
            if (childTimeSlot != null)
            {
                throw new Exception($"Slot này đã được đăng kí");
            }


            /// check bác sĩ trùng slot
            var doctor = await _doctorServices.GetSutableDoctorAsync(createAppointment.SlotNumber, createAppointment.Date);
            if (doctor == null)
            {
                throw new Exception($"Đã hết bác sĩ làm ca {createAppointment.SlotNumber} ");
            }

            //-------------- trạng thái cho các field liên quan --------------
            var appointment = new Appointment();
            appointment.ChildID = child.ChildID;
            appointment.DoctorID = doctor.DoctorID;
            appointment.VaccineID = createAppointment.VaccineID;
            appointment.TimeSlotID = timeSlot.TimeSlotID;
            appointment.Status = "PENDING";

            ///câp nhật timeslot cho trẻ 
            await _childServices.CreateChildTimeSlot(createAppointment.SlotNumber, createAppointment.Date, createAppointment.ChildID);

            ///cập nhật timeslot Doctor - gom lại thành func trong doctorServices
            var doctorTimeSlot = await _doctorServices.FindDoctorTimeSlotAsync(doctor.DoctorID, createAppointment.Date, createAppointment.SlotNumber);
            var docSlot = await _doctorServices.SetDoctorTimeSlotAsync(doctorTimeSlot, false);
            doctorTimeSlot.Available = false;

            await _doctorServices.UpdateDoctorScheduleAsync(doctorTimeSlot);

            ///cập nhật lại số lượng vaccine - gon lại thành func trong vaccineServices
            var vaccine = await _vaccineServices.GetVaccineByIDAsync(createAppointment.VaccineID);
            vaccine.Stock -= 1;
            vaccine = await _vaccineRepository.UpdateVaccineAsync(vaccine);


            return await _appointmentRepository.CreateAppointmentAsync(appointment);
        }


        /// <summary>
        /// hàm lấy danh sách lịch hẹn theo ID và role
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<AppointmentDto>> GetAppointmentListByIDAsync(int id, string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                throw new Exception("Role can't be empty");
            }
            if (id == 0)
            {
                throw new Exception("ID can't be empty");
            }

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
                    throw new Exception("Role không hợp lệ <child, doctor>");
            }
            List<AppointmentDto> appointmentDtoList = new List<AppointmentDto>();
            foreach (var appointment in appointmentList)
            {
                var appointmentDto = new AppointmentDto
                {
                    AppointmentID = appointment.AppointmentID,
                    Child = $"{appointment.Child.Firstname} {appointment.Child.Lastname}",
                    Doctor = $"{appointment.Doctor.Account.Firstname} {appointment.Doctor.Account.Lastname}",
                    VaccineID = appointment.VaccineID,
                    Date = appointment.TimeSlots.DailySchedule.AppointmentDate,
                    Status = appointment.Status
                };
                appointmentDtoList.Add(appointmentDto);
            }

            return appointmentDtoList;
        }

        public async Task SetOverdueAppointmentAsync()
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


        public async Task<Appointment?> UpdateAppointmentAsync(int appointmentID, UpdateAppointmentDto modAppointment)
        {
            var appointment = await _appointmentRepository.GetAppointmentByID(appointmentID);

            if (appointment == null)
            {
                throw new Exception("Lịch hẹn không tồn tại!");
            }

            var tempAppoint = new UpdateAppointmentDto();
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
                throw new Exception("ngày này đã qua hạn!");
            }

            ///kiểm tra slot
            var timeSlot = await _timeSlotServices.GetTimeSlotAsync(tempAppoint.SlotNumber, tempAppoint.Date);
            if (timeSlot.Available == false)
            {
                throw new Exception("Slot này đã quá hạn!");
            }

            ///kiểm DoctorSlot
            var doctorTimeSlot = await _doctorServices.FindDoctorTimeSlotAsync(tempAppoint.DoctorID, tempAppoint.Date, tempAppoint.SlotNumber);
            if (doctorTimeSlot == null || doctorTimeSlot.Available == false)
            {
                throw new Exception($"không có bác sĩ vào ngày {appointment.TimeSlots.DailySchedule.AppointmentDate} slot {modAppointment.SlotNumber}");
            }

            ///kiểm tra slot này đã được đăng kí chưa
            var childTimeSlot = await _childServices.GetChildTimeSlotBySlotNumberAsync(tempAppoint.SlotNumber, tempAppoint.Date);
            if (childTimeSlot == null)
            {
                throw new Exception("Bạn đã đăng kí Slot này");
            }

            ///kiểm tra vaccine
            var vaccine = await _vaccineServices.GetVaccineByIDAsync(tempAppoint.VaccineID);
            if (vaccine.Stock == 0)
            {
                throw new Exception("Vaccine này đã hết");
            }

            //cập nhật lại các timeslot cho doctor và child
            ///cập nhật lại slot cho doctor
            if (modAppointment.DoctorID != default && appointment.DoctorID != modAppointment.DoctorID)
            {
                var oldDoctorSlot = await _doctorServices.FindDoctorTimeSlotAsync(appointment.DoctorID, appointment.TimeSlots.DailySchedule.AppointmentDate, appointment.TimeSlots.SlotNumber);
                if (oldDoctorSlot != null)
                {
                    await _doctorServices.SetDoctorTimeSlotAsync(oldDoctorSlot, true);
                }

                var newDoctorSlot = await _doctorServices.FindDoctorTimeSlotAsync(modAppointment.DoctorID, tempAppoint.Date, tempAppoint.SlotNumber);
                if (newDoctorSlot != null)
                {
                    await _doctorServices.SetDoctorTimeSlotAsync(newDoctorSlot, false);
                }
                appointment.DoctorID = modAppointment.DoctorID;
            }
            ///cập nhật lại vaccine
            if (modAppointment.VaccineID != default && appointment.VaccineID != modAppointment.VaccineID)
            {
                var oldVaccine = await _vaccineServices.GetVaccineByIDAsync(appointment.VaccineID);
                if (oldVaccine != null)
                {
                    oldVaccine.Stock += 1;
                    await _vaccineRepository.UpdateVaccineAsync(oldVaccine);
                }

                vaccine.Stock -= 1;
                await _vaccineRepository.UpdateVaccineAsync(vaccine);

                appointment.VaccineID = modAppointment.VaccineID;
            }

            ///cập nhật lại slot cho child
            if (modAppointment.SlotNumber != default && appointment.TimeSlots.SlotNumber != modAppointment.SlotNumber
                && modAppointment.Date != default && appointment.TimeSlots.DailySchedule.AppointmentDate != modAppointment.Date)
            {
                var oldChildSlot = await _childServices.GetChildTimeSlotBySlotNumberAsync(appointment.TimeSlots.SlotNumber, appointment.TimeSlots.DailySchedule.AppointmentDate);
                if (oldChildSlot != null)
                {
                    oldChildSlot.Available = false;
                }

                await _childServices.CreateChildTimeSlot(tempAppoint.SlotNumber, tempAppoint.Date, appointment.ChildID);
            }




            return await _appointmentRepository.UpdateAppointmentAsync(appointment);
        }
    }
}
