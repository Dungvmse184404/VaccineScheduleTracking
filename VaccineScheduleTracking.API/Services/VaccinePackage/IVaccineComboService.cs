using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Services.VaccinePackage
{
    public interface IVaccineComboService
    {
        Task<List<VaccineCombo>> GetVaccineCombosAsync();
    }
}
