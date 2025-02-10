using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Services
{
    public interface IVaccineService
    {
        // Vaccine funtion
        Task<List<Vaccine>> GetVaccinesAsync(FilterVaccineDto filterVaccineDto);
        Task<Vaccine?> CreateVaccineAsync(AddVaccineDto addVaccineDto);
        Task<Vaccine?> UpdateVaccineAsync(int id, UpdateVaccineDto updateVaccineDto); 
        Task<Vaccine?> DeleteVaccineAsync(int id);

        // VaccineType functin
        Task<VaccineType?> CreateVaccineTypeAsync(AddVaccineTypeDto addVaccineTypeDto);
        
    }
}
