using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Appointments;
using VaccineScheduleTracking.API_Test.Models.Entities;
using static VaccineScheduleTracking.API_Test.Services.Appointments.AppointmentService;

namespace VaccineScheduleTracking.API_Test.Services.Appointments
{
    public interface IAppointmentService
    {
        Task<Appointment?> GetAppointmentByIDAsync(int appointmentID);
        Task<List<Appointment>> GetDoctorAppointmentsAsync(int doctorId);
        Task<List<Appointment>> GetChildAppointmentsAsync(int childId);
        Task<List<Appointment>> GetPendingDoctorAppointmentAsync(int doctorId);
        Task<List<Appointment>> GetPendingAppointments(int beforeDueDate);

        //---------------------- functions ----------------------
        Task<Result<Appointment>> CreateAppointmentAsync(CreateAppointmentDto createAppointment);
        Task<Appointment?> UpdateAppointmentAsync(int appointmenID, UpdateAppointmentDto appointment);
        Task<Appointment?> SetAppointmentStatusAsync(int appointmentId, string status, string? note);
        Task AddAppointmentToRecord(Appointment appointment, string? note);
        Task<Appointment?> CancelAppointmentAsync(int appointmentId, string reason);
        Task<CancelAppointment> GetCancelAppointmentReasonAsync(int appointmentId);
        Task SetOverdueAppointmentAsync(int threshold);
        //Task ValidateVaccineConditions(int vaccineId, int childId, DateOnly date);
        Task<DateOnly?> GetLatestVaccineDate(int childId, int vaccineId);
        Task<List<string>> ValidateAppointmentConditions(int childID, int vaccineID, int slotNumber, DateOnly date);
        Task<Appointment> FindAppointment(CreateAppointmentDto appointmentDto);

    }
}
