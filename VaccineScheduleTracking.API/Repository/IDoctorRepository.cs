using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository
{
    public interface IDoctorRepository
    {
        Task<List<Doctor>> GetAllDoctorAsync();
        Task<Doctor?> GetSuitableDoctor(int slot, DateTime time);
        Task<Doctor?> GetDoctorByIDAsync(int doctorID);

    }
}
