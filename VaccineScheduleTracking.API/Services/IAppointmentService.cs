using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Services
{
    public interface IAppointmentService
    {
        Task<Appointment?> CreateAppointmentAsync(CreateAppointmentDto appointment);
        Task<List<Appointment>> GetAppointmentListByIDAsync(AppointmentDto appointment);
        Task<Appointment?> ModifyAppointmentAsync(AppointmentDto appointment);
    }
}
