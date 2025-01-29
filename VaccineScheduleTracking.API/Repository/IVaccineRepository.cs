using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Repository
{
    public interface IVaccineRepository
    {
        Task<List<Vaccine>> GetVaccinesAsync(FilterVaccineDto filterVaccineDto);
    }
}
