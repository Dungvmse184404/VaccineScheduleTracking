using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Repository
{
    public interface IDoctorRepository
    {
        Task<List<Doctor>> GetAllDoctorAsync();
        Task<Doctor?> GetSuitableDoctor(int slot, DateTime time);
        // Remove Doctor authorize
    }
}
