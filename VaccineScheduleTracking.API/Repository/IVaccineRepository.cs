using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Repository
{
    public interface IVaccineRepository
    {
        Task<List<Vaccine>> GetVaccinesAsync(FilterVaccineDto filterVaccineDto);
        Task<VaccineType?> GetVaccineTypeByNameAsync(string name);
        Task<VaccineType> AddVaccineTypeAsync(VaccineType vaccineType);
    }
}
