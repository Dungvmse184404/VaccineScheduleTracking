using AutoMapper;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API.Repository;

namespace VaccineScheduleTracking.API.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<Appointment?> CreateAppointmentAsync(Appointment appointment)
        {
            throw new NotImplementedException();
        }

        public Task<Appointment?> ModifyAppointmentAsync(Appointment appointment)
        {
            throw new NotImplementedException();
        }

        public Task<Appointment?> DeleteAppointmentAsync(Appointment appointment)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Appointment>> GetAllAppointmentAsync(AppointmentDto appointmentDto)
        {
            return await _appointmentRepository.GetAllAppointmentAsync(appointmentDto);
        }
    }
}
