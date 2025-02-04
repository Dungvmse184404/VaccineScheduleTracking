using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Repository
{
    public interface IAppointmentRepository
    {
        Task<Appointment?> CreateAppointmentAsync(Appointment appointment);
        Task<List<Appointment>> GetAllAppointmentAsync(AppointmentDto appointment);
        Task<Appointment?> ModifyAppointmentAsync(AppointmentDto appointment);

    }
   
}
