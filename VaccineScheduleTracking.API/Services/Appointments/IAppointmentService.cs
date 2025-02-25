using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Appointments;

namespace VaccineScheduleTracking.API_Test.Services.Appointments
{
    public interface IAppointmentService
    {
        Task<Appointment?> CreateAppointmentAsync(CreateAppointmentDto appointment);
        Task<List<AppointmentDto>> GetAppointmentListByIDAsync(int id, string role);
        Task<Appointment?> UpdateAppointmentAsync(UpdateAppointmentDto appointment);
        Task SetOverdueAppointmentAsync();
    }
}
