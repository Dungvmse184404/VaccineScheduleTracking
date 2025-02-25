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

            var child = await _childServices.GetChildByIDAsync(createAppointment.ChildID);
            if (child == null)
            {
                throw new Exception($"Không tìm thấy trẻ có ID {createAppointment.ChildID}");
            }
            ///tạo ChildTimeSlot - đã xong
            var childTimeSlot = await _childServices.CreateChildTimeSlot(createAppointment.SlotNumber, createAppointment.Date, createAppointment.ChildID);



            /// check bác sĩ trùng slot
            var doctor = await _doctorServices.GetSutableDoctorAsync(createAppointment.SlotNumber, createAppointment.Date);
            if (doctor == null)
            {
                throw new Exception($"Đã hết bác sĩ làm ca {createAppointment.SlotNumber} ");
            }

            //------------------- trạng thái cho các field liên quan ------------------
            var appointment = new Appointment();
            appointment.ChildID = child.ChildID;
            appointment.DoctorID = doctor.DoctorID;
            appointment.VaccineID = createAppointment.VaccineID;
            appointment.TimeSlotID = timeSlot.TimeSlotID;
            appointment.Status = "PENDING";

            ///cập nhật timeslot Doctor
            var doctorTimeSlot = await _doctorServices.FindDoctorTimeSlotAsync(doctor.DoctorID, createAppointment.Date, createAppointment.SlotNumber);
            doctorTimeSlot.Available = false;
            await _doctorServices.UpdateDoctorScheduleAsync(doctorTimeSlot);

            ///cập nhật lại số lượng vaccine
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
                    throw new Exception("Invalid role specified");
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
        public async Task<Appointment?> UpdateAppointmentAsync(UpdateAppointmentDto modifyAppointment)
        {
            var appointment = await _appointmentRepository.GetAppointmentByID(modifyAppointment.AppointmentID);
            if (appointment == null)
            {
                throw new Exception("Appointment does not exist!");
            }
            appointment.ChildID = NullValidator(modifyAppointment.ChildID)
                ? modifyAppointment.ChildID
                : appointment.ChildID;

            appointment.DoctorID = NullValidator(modifyAppointment.DoctorID)
                ? modifyAppointment.DoctorID
                : appointment.DoctorID;

            appointment.VaccineID = NullValidator(modifyAppointment.VaccineID)
                ? modifyAppointment.VaccineID
                : appointment.VaccineID;
            /// chưa test chức năng này
            appointment.TimeSlotID = NullValidator(modifyAppointment.TimeSlotID)
              ? modifyAppointment.TimeSlotID
              : appointment.TimeSlotID;
            appointment.Status = NullValidator(modifyAppointment.Status)
                ? modifyAppointment.Status
                : appointment.Status;

            return await _appointmentRepository.UpdateAppointmentAsync(appointment);
        }
    }
}
