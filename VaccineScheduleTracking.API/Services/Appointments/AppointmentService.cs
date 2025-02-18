using AutoMapper;
using VaccineScheduleTracking.API.Helpers;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Appointments;
using VaccineScheduleTracking.API_Test.Repository;
using VaccineScheduleTracking.API_Test.Repository.Appointments;
using VaccineScheduleTracking.API_Test.Repository.Children;
using VaccineScheduleTracking.API_Test.Repository.Vaccines;
using VaccineScheduleTracking.API_Test.Services.Appointments;

namespace VaccineScheduleTracking.API_Test.Services.Appointments
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ITimeSlotRepository _timeSlotRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IVaccineRepository _vaccineRepository;
        private readonly IChildRepository _childRepository;
        private readonly IMapper _mapper;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IMapper mapper,
            IVaccineRepository vaccineRepository,
            ITimeSlotRepository timeSlotRepository,
            IDoctorRepository doctorRepository,
            IChildRepository childRepository)
        {
            _appointmentRepository = appointmentRepository;
            _vaccineRepository = vaccineRepository;
            _timeSlotRepository = timeSlotRepository;
            _doctorRepository = doctorRepository;
            _childRepository = childRepository;
            _mapper = mapper;
        }

        public async Task<Appointment?> CreateAppointmentAsync(CreateAppointmentDto createAppointment)
        {
            //_slotRepository.GenerateSlotsForMonth(6);// tạo lịch độ dài 6 tháng để khách book
            var appointment = _mapper.Map<Appointment>(createAppointment);
            //-------------------------------

            ///check ca tiêm đặt chưa
            var timeSlot = await _timeSlotRepository.GetTimeSlotAsync(createAppointment.TimeSlot, createAppointment.Date);
            if (timeSlot.Available == false)
            {
                throw new Exception("this slot is already taken.");
            }

            /// check bác sĩ trùng slot
            //var doctor = await _doctorRepository.GetSuitableDoctor(createAppointment.Slot, createAppointment.Time);
            //if (doctor == null)
            //{
            //    throw new Exception("can't find sutable doctor");
            //}


            /// check tuổi của child (chưa check phản ứng phụ)
            var vaccine = await _vaccineRepository.GetSutableVaccine(appointment.Child.Age, appointment.VaccineType.Name);
            if (vaccine == null)
            {
                throw new Exception("No suitable vaccine available");
            }


            //-------------------------------
            appointment.ChildID = createAppointment.ChildID;
            //appointment.DoctorID = doctor.DoctorID;
            appointment.VaccineTypeID = vaccine.VaccineTypeID;
            appointment.TimeSlotID = timeSlot.TimeSlotID;
            appointment.Status = "PENDING";

            timeSlot = await _timeSlotRepository.ChangeTimeSlotStatus(timeSlot.TimeSlotID, false);
            return await _appointmentRepository.CreateAppointmentAsync(appointment);
        }

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
                    VaccineType = appointment.VaccineType.Name,
                    Date = appointment.TimeSlots.DailySchedule.AppointmentDate,
                    Status = appointment.Status
                };
                appointmentDtoList.Add(appointmentDto);
            }

            return appointmentDtoList;
        }

        public async Task<Appointment?> UpdateAppointmentAsync(UpdateAppointmentDto modifyAppointment)
        {
            var appointment = await _appointmentRepository.UpdateAppointmentAsync(modifyAppointment);
            if (appointment == null)
            {
                throw new Exception("Appointment does not exist!");
            }
            appointment.ChildID = ValidationHelper.NullValidator(modifyAppointment.ChildID)
                ? modifyAppointment.ChildID
                : appointment.ChildID;

            appointment.DoctorID = ValidationHelper.NullValidator(modifyAppointment.DoctorID)
                ? modifyAppointment.DoctorID
                : appointment.DoctorID;

            appointment.VaccineTypeID = ValidationHelper.NullValidator(modifyAppointment.VaccineTypeID)
                ? modifyAppointment.VaccineTypeID
                : appointment.VaccineTypeID;
            /// đang chạy không đúng dự tínhq
            //appointment.Slot = ValidationHelper.NullValidator(modifyAppointment.Slot)
            //  ? modifyAppointment.Slot
            //  : appointment.Slot;

            appointment.Status = ValidationHelper.NullValidator(modifyAppointment.Status)
                ? modifyAppointment.Status
                : appointment.Status;

            return appointment;
        }
    }
}
