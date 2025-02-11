using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs;

namespace VaccineScheduleTracking.API.Services
{
    public interface IVaccineService
    {
        // Vaccine funtion
        Task<List<Vaccine>> GetVaccinesAsync(FilterVaccineDto filterVaccineDto);
        Task<Vaccine?> CreateVaccineAsync(AddVaccineDto addVaccineDto);
        Task<Vaccine?> UpdateVaccineAsync(int id, UpdateVaccineDto updateVaccineDto); 
        Task<Vaccine?> DeleteVaccineAsync(int id);

        // VaccineType function
        Task<VaccineType?> CreateVaccineTypeAsync(AddVaccineTypeDto addVaccineTypeDto);
        Task<VaccineType?> UpdateVaccineTypeAsync(int id, UpdateVaccineTypeDto updateVaccineTypeDto);
        Task<VaccineType?> DeleteVaccineTypeAsync(int id);
        
    }
}
