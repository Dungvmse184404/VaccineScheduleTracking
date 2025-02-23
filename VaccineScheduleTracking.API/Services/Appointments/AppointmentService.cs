using AutoMapper;
using VaccineScheduleTracking.API.Helpers;
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
using VaccineScheduleTracking.API_Test.Services.DailyTimeSlots;
using VaccineScheduleTracking.API_Test.Services.Vaccines;

namespace VaccineScheduleTracking.API_Test.Services.Appointments
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IVaccineService _vaccineServices;
        private readonly IDoctorServices _doctorServices;
        private readonly ITimeSlotServices _timeSlotServices;
        private readonly ITimeSlotRepository _timeSlotRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IVaccineRepository _vaccineRepository;
        private readonly IChildRepository _childRepository;
        private readonly IMapper _mapper;

        public AppointmentService(
            IVaccineService vaccineServices,
            IAppointmentRepository appointmentRepository,
            IMapper mapper,
            IVaccineRepository vaccineRepository,
            ITimeSlotRepository timeSlotRepository,
            IDoctorRepository doctorRepository,
            IChildRepository childRepository,
            ITimeSlotServices timeSlotServices,
            IDoctorServices doctorServices)
        {
            _vaccineServices = vaccineServices;
            _appointmentRepository = appointmentRepository;
            _vaccineRepository = vaccineRepository;
            _timeSlotRepository = timeSlotRepository;
            _doctorRepository = doctorRepository;
            _childRepository = childRepository;
            _mapper = mapper;
            _timeSlotServices = timeSlotServices;
            _doctorServices = doctorServices;
        }

        public async Task<Appointment?> CreateAppointmentAsync(CreateAppointmentDto createAppointment)
        {
            int Days = 6;
             
            /// tạo lịch
            await _timeSlotServices.GenerateCalanderAsync(Days);

            /// tạo lịch làm vc cho bác sĩ
            var doctorList = await _doctorRepository.GetAllDoctorAsync();
            await _doctorServices.GenerateDoctorCalanderAsync(doctorList, Days);

            //var appointment = _mapper.Map<Appointment>(createAppointment);
            //-------------------------------

            ///check ca tiêm đặt chưa

            if (!TimeSlotHelper.ExcludedDay(createAppointment.Date))
            {
                throw new Exception("Chủ nhật hong có làm việc");
            }
            var timeSlot = await _timeSlotRepository.GetTimeSlotAsync(createAppointment.SlotNumber, createAppointment.Date);
            if (timeSlot.Available == false)
            {
                throw new Exception("this slot is already taken.");
            }

            /// check tuổi của child (chưa check phản ứng phụ)
            
            Child child = await _childRepository.GetChildByIDAsync(createAppointment.ChildID);
            var vaccine = await _vaccineServices.GetSutableVaccineAsync(child.Age, createAppointment.VaccineID);
            if (vaccine == null)
            {
                throw new Exception("No suitable vaccine available");
            } 
            
            
            /// check bác sĩ trùng slot
            var doctor = await _doctorServices.GetSutableDoctorAsync(createAppointment.SlotNumber, createAppointment.Date);//đang lỗi ko tìm được doctor
            if (doctor == null)
            {
                throw new Exception("can't find sutable doctor");
            }


            //-------------------------------
            var appointment = new Appointment();
            appointment.ChildID = createAppointment.ChildID;
            appointment.DoctorID = doctor.DoctorID;
            appointment.VaccineTypeID = vaccine.VaccineTypeID;
            appointment.TimeSlotID = timeSlot.TimeSlotID;
            appointment.Status = "PENDING";

            ///set status của timeSlot 
            timeSlot.Available = false;
            timeSlot = await _timeSlotServices.UpdateTimeSlotAsync(timeSlot);

            ///set status của doctorTimeSlot   - chắc ko cần nữa 
            
            //doctor = await _doctorRepository.UpdateDoctorTimeSlotAsync();

            ///cập nhật lại số lượng vaccine
            vaccine.Stock -= 1;
            vaccine = await _vaccineRepository.UpdateVaccineAsync(vaccine);


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
