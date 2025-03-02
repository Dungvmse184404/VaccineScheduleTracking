using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Appointments;

namespace VaccineScheduleTracking.API_Test.Repository.Appointments
{
    public interface IAppointmentRepository
    {
        Task<List<Appointment>> GetAppointmentListByDoctorIDAsync(int id);
        Task<List<Appointment>> GetAppointmentListByChildIDAsync(int id);
        Task<Appointment?> GetAppointmentByIDAsync(int id);
        Task<List<Appointment>> GetAppointmentListByStatus(string status);
        Task<Appointment?> CreateAppointmentAsync(Appointment Appointment);
        Task<List<Appointment>> SearchAppointmentByKeyword(string keyword);
        Task<Appointment?> UpdateAppointmentAsync(Appointment appointment);
        Task<List<Appointment>> GetAllAppointmentsAsync();
    }

}
