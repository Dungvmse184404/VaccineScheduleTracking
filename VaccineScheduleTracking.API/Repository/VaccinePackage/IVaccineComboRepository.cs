using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository.VaccinePackage
{
    public interface IVaccineComboRepository
    {
        Task<List<VaccineCombo>> GetVaccineCombosAsync();
        Task<VaccineCombo> AddVaccineComboAsync(VaccineCombo vaccineCombo);
        Task<VaccineCombo?> GetVaccineComboByIdAsync(int id);
        Task<VaccineCombo?> DeleteVaccineComboAsync(int id);
    }
}
