using Microsoft.EntityFrameworkCore;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Appointments;
using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository.Appointments
{
    public interface IAppointmentRepository
    {
        Task<List<Appointment>> GetAppointmentsByDoctorIDAsync(int id);
        Task<List<Appointment>> GetAppointmentsByChildIDAsync(int id);
        Task<Appointment?> GetAppointmentByIDAsync(int id);
        Task<List<Appointment>> GetAppointmentListByStatus(string status);
        Task<List<Appointment>> GetPendingDoctorAppointmentAsync(int doctorId);
        
        Task<Appointment?> CreateAppointmentAsync(Appointment Appointment);
        Task<List<Appointment>> SearchAppointmentByKeyword(string keyword);
        Task<Appointment?> UpdateAppointmentAsync(Appointment appointment);
        Task<List<Appointment>> GetAllAppointmentsAsync();
        Task<List<Appointment>> GetAppointmentByDateAsync(int childId, DateOnly date);
        Task createCancelReasonAsync(CancelReason reason);
    }

}
