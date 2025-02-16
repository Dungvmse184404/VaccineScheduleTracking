using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Repository
{
    public interface IAppointmentRepository
    {
        Task<List<Appointment>> GetAppointmentListByDoctorIDAsync(int id);
        Task<List<Appointment>> GetAppointmentListByChildIDAsync(int id);
        Task<Appointment> GetAppointmentByID(int id);
        Task<List<Appointment>> GetAppointmentListByStatus(string status);
        Task<Appointment?> CreateAppointmentAsync(Appointment Appointment);
        Task<List<Appointment>> SearchAppointmentByKeyword(string keyword);
        Task<Appointment?> UpdateAppointmentAsync(UpdateAppointmentDto appointment);

    }
   
}
