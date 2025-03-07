using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Appointments;

namespace VaccineScheduleTracking.API_Test.Services.Appointments
{
    public interface IAppointmentService
    {
        Task<Appointment?> GetAppointmentByIDAsync(int appointmentID);
        Task<Appointment?> CreateAppointmentAsync(CreateAppointmentDto appointment);
        Task<List<Appointment>> GetDoctorAppointmentsAsync(int doctorId);
        Task<List<Appointment>> GetChildAppointmentsAsync(int childId);
        Task<List<Appointment>> GetPendingDoctorAppointmentAsync(int doctorId);
        Task<Appointment?> UpdateAppointmentAsync(int appointmentId, UpdateAppointmentDto appointment);

        Task<Appointment?> SetAppointmentStatusAsync(int appointmentId, string status);
        Task<Appointment?> CancelAppointmentAsync(int appointmentId);
        Task SetOverdueAppointmentAsync();
    }
}
