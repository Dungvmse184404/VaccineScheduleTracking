using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Repository
{
    public interface IVaccineRepository
    {
        // Vaccine function
        Task<List<Vaccine>> GetVaccinesAsync(FilterVaccineDto filterVaccineDto);
        Task<Vaccine?> GetVaccineByIDAsync(int id);
        Task<Vaccine?> GetVaccineByNameAsync(string name);
        Task<Vaccine> AddVaccineAsync(Vaccine vaccine);
        Task<Vaccine> GetSutableVaccine(int Age, string TypeName);
        //Task<Vaccine> UpdateVaccineAsync(int id);
        Task<Vaccine> DeleteVaccineByIDAsync(Vaccine vaccine);
        // VaccineType function
        Task<VaccineType?> GetVaccineTypeByNameAsync(string name);
        Task<VaccineType> AddVaccineTypeAsync(VaccineType vaccineType);
        
    }
}
