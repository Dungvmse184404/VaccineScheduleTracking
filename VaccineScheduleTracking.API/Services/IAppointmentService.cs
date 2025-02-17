using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Appointments;

namespace VaccineScheduleTracking.API_Test.Services
{
    public interface IAppointmentService
    {
        Task<Appointment?> CreateAppointmentAsync(CreateAppointmentDto appointment);
        Task<List<Appointment>> GetAppointmentListByIDAsync(int id, string role);
        Task<Appointment?> UpdateAppointmentAsync(UpdateAppointmentDto appointment);
    }
}
