using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Services
{
    public interface IVaccineService
    {
        Task<List<Vaccine>> GetVaccinesAsync(FilterVaccineDto filterVaccineDto);
    }
}
