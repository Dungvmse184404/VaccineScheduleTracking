using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository.IRepository
{
    public interface IDoctorRepository
    {
        Task<List<Doctor>> GetAllDoctorAsync();
        Task<Doctor?> GetSuitableDoctor(int slot, DateTime time);


    }
}
