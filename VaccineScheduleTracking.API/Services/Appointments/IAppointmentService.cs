using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Appointments;

namespace VaccineScheduleTracking.API_Test.Services.Appointments
{
    public interface IAppointmentService
    {
        Task<Appointment?> CreateAppointmentAsync(CreateAppointmentDto appointment);
        Task<List<Appointment>> GetAppointmentListByIDAsync(int id, string role);
        Task<Appointment?> UpdateAppointmentAsync(int appointmentId, UpdateAppointmentDto appointment);

        Task<Appointment?> CancelAppointmentAsync(int appointmentId);
        Task SetOverdueAppointmentAsync();
    }
}
