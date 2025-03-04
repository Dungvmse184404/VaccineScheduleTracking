using VaccineScheduleTracking.API_Test.Models.DTOs;
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

        public async Task<VaccineCombo> CreateVaccineComboAsync(CreateVaccineComboDto createVaccineCombo)
        {
            var vaccineCombo = new VaccineCombo();
            vaccineCombo.Name = createVaccineCombo.Name;
            vaccineCombo.Description = createVaccineCombo.Description;

            vaccineCombo = await vaccineComboRepository.AddVaccineComboAsync(vaccineCombo);

            return vaccineCombo;
        }

        public async Task<List<VaccineCombo>> GetVaccineCombosAsync()
        {
            return await vaccineComboRepository.GetVaccineCombosAsync();
        }


    }
}
