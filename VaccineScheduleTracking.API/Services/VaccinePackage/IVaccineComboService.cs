using VaccineScheduleTracking.API_Test.Models.DTOs;
using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Services.VaccinePackage
{
    public interface IVaccineComboService
    {
        Task<List<VaccineCombo>> GetVaccineCombosAsync();
        Task<VaccineCombo> CreateVaccineComboAsync(CreateVaccineComboDto createVaccineCombo);
        Task<VaccineCombo?> GetVaccineComboByIdAsync(int id);
        Task<VaccineContainer> AddVaccineContainerAsync(CreateVaccineContainerDto createVaccineContainer);
        Task<bool> DeleteVaccineContainerAsync(DeleteVaccineContainerDto deleteVaccineContainer);
        Task<bool> DeleteVaccineComboAsync(int id);

        Task RegisterCombo(DateOnly startDate, int childId, int ComboId);
    }
}
