using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs;

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
        Task<Vaccine> UpdateVaccineAsync(Vaccine vaccine);
        Task<Vaccine> DeleteVaccineAsync(Vaccine vaccine);
        // VaccineType function
        Task<VaccineType?> GetVaccineTypeByNameAsync(string name);
        Task<VaccineType> GetVaccineTypeByIDAsync(int id);
        Task<VaccineType> AddVaccineTypeAsync(VaccineType vaccineType);
        Task<VaccineType> UpdateVaccineTypeAsync(VaccineType vaccineType);
        Task<VaccineType> DeleteVaccineTypeAsync(VaccineType vaccineType);


    }
}
