using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API_Test.Repository.VaccinePackage;

namespace VaccineScheduleTracking.API_Test.Services.VaccinePackage
{
    public class VaccineComboService : IVaccineComboService
    {
        private readonly IVaccineComboRepository vaccineComboRepository;

        public VaccineComboService(IVaccineComboRepository vaccineComboRepository)
        {
            this.vaccineComboRepository = vaccineComboRepository;
        }

        public async Task<List<VaccineCombo>> GetVaccineCombosAsync()
        {
            return await vaccineComboRepository.GetVaccineCombosAsync();
        }
    }
}
