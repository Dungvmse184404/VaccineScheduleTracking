using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Services
{
    public interface IAppointmentService
    {
        Task<Appointment?> CreateAppointmentAsync(Appointment appointment);
        Task<List<Appointment>> GetAllAppointmentAsync(AppointmentDto appointment);
        Task<Appointment?> ModifyAppointmentAsync(Appointment appointment);
        Task<Appointment?> DeleteAppointmentAsync(Appointment appointment);
    }
}
