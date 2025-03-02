using System.Reflection.PortableExecutable;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs;
using VaccineScheduleTracking.API_Test.Models.DTOs.Vaccines;

namespace VaccineScheduleTracking.API_Test.Repository.Vaccines
{
    public interface IVaccineRepository
    {
        // Vaccine function
        Task<List<Vaccine>> GetVaccineByTypeIDAsync(int typeID);
        Task<List<Vaccine>> GetVaccinesAsync(FilterVaccineDto filterVaccineDto);
        Task<Vaccine?> GetVaccineByIDAsync(int id);
        Task<Vaccine?> GetVaccineByNameAsync(string name);
        Task<List<Vaccine>> GetVaccineListByVaccineTypeAsync(int id);
        Task<Vaccine> AddVaccineAsync(Vaccine vaccine);
        Task<Vaccine> UpdateVaccineAsync(Vaccine vaccine);
        Task<Vaccine> DeleteVaccineAsync(Vaccine vaccine);
        // VaccineType function
        Task<VaccineType?> GetVaccineTypeByNameAsync(string name);

        Task<VaccineType> GetVaccineTypeByIDAsync(int id);
        Task<VaccineType> AddVaccineTypeAsync(VaccineType vaccineType);
        Task<VaccineType> UpdateVaccineTypeAsync(VaccineType vaccineType);
        Task<VaccineType> DeleteVaccineTypeAsync(VaccineType vaccineType);
        Task<List<VaccineType>> GetVaccinesTypeAsync();

    }
}
