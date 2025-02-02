using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Repository
{
    public interface IVaccineRepository
    {
        // Vaccine function
        Task<List<Vaccine>> GetVaccinesAsync(FilterVaccineDto filterVaccineDto);
        Task<Vaccine?> GetVaccineByNameAsync(string name);
        Task<Vaccine> AddVaccineAsync(Vaccine vaccine);

        // VaccineType function
        Task<VaccineType?> GetVaccineTypeByNameAsync(string name);
        Task<VaccineType> AddVaccineTypeAsync(VaccineType vaccineType);
        
    }
}
