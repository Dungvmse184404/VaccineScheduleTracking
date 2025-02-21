using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Services
{
    public interface IDoctorServices
    {
        Task GenerateDoctorCalanderAsync(List<Doctor> doctorList, int numberOfDays);
    }
}
