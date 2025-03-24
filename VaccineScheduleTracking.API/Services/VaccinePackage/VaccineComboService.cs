using VaccineScheduleTracking.API_Test.Models.DTOs;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API_Test.Repository.VaccineContainers;
using VaccineScheduleTracking.API_Test.Repository.VaccinePackage;
using VaccineScheduleTracking.API_Test.Repository.Vaccines;

namespace VaccineScheduleTracking.API_Test.Services.VaccinePackage
{
    public class VaccineComboService : IVaccineComboService
    {
        private readonly IVaccineComboRepository vaccineComboRepository;
        private readonly IVaccineRepository vaccineRepository;
        private readonly IVaccineContainerRepository vaccineContainerRepository;

        public VaccineComboService(IVaccineComboRepository vaccineComboRepository, IVaccineRepository vaccineRepository, IVaccineContainerRepository vaccineContainerRepository)
        {
            this.vaccineComboRepository = vaccineComboRepository;
            this.vaccineRepository = vaccineRepository;
            this.vaccineContainerRepository = vaccineContainerRepository;
        }

        public async Task<VaccineContainer> AddVaccineContainerAsync(CreateVaccineContainerDto createVaccineContainer)
        {
            var vaccineCombo = await GetVaccineComboByIdAsync(createVaccineContainer.VaccineComboID);
            if (vaccineCombo == null)
            {
                throw new Exception("Không tìm thấy Vaccine combo hợp lệ");
            }
            var vaccine = await vaccineRepository.GetVaccineByIDAsync(createVaccineContainer.VaccineID);
            if (vaccine == null)
            {
                throw new Exception("Không tìm thấy Vaccine hợp lệ");
            }
            var contaminatedVaccine = vaccineCombo.VaccineContainers.FirstOrDefault(x => x.Vaccine.VaccineTypeID == vaccine.VaccineTypeID);
            if (contaminatedVaccine != null)
            {
                throw new Exception($"Vaccine combo đã có vaccine {contaminatedVaccine.Vaccine.Name} cho bệnh {contaminatedVaccine.Vaccine.VaccineType.Name}");
            }
            var vaccineContainer = new VaccineContainer();
            vaccineContainer.VaccineComboID = createVaccineContainer.VaccineComboID;
            vaccineContainer.VaccineID = createVaccineContainer.VaccineID;
            return await vaccineContainerRepository.AddVaccineContainer(vaccineContainer);
        }

       

        public async Task<VaccineCombo> CreateVaccineComboAsync(CreateVaccineComboDto createVaccineCombo)
        {
            var vaccineCombo = new VaccineCombo();
            vaccineCombo.Name = createVaccineCombo.Name;
            vaccineCombo.Description = createVaccineCombo.Description;

            vaccineCombo = await vaccineComboRepository.AddVaccineComboAsync(vaccineCombo);

            return vaccineCombo;
        }

        public async Task<bool> DeleteVaccineComboAsync(int id)
        {
            var vaccineCombo = await vaccineComboRepository.GetVaccineComboByIdAsync(id);
            if (vaccineCombo == null)
            {
                throw new Exception("Không tìm thấy vaccine combo hợp lệ");
            }
            await vaccineComboRepository.DeleteVaccineComboAsync(id);
            return true;
        }

        public async Task<bool> DeleteVaccineContainerAsync(DeleteVaccineContainerDto deleteVaccineContainer)
        {
            var vaccineCombo = await GetVaccineComboByIdAsync(deleteVaccineContainer.VaccineComboID);
            if (vaccineCombo == null)
            {
                throw new Exception("Không tìm thấy Vaccine combo hợp lệ");
            }
            var deletedVaccineContainer = await vaccineContainerRepository.GetVaccineContainerByIdAsync(deleteVaccineContainer.VaccineContainerID);
            if (deletedVaccineContainer == null
                || vaccineCombo.VaccineContainers.FirstOrDefault(x => x.VaccineContainerID == deletedVaccineContainer.VaccineContainerID) == null)
            {
                throw new Exception("Không tìm thấy Vaccine container hợp lệ");
            }
            await vaccineContainerRepository.DeleteVaccineContainerAsync(deletedVaccineContainer.VaccineID);    
            return true;
        }

        public async Task<VaccineCombo?> GetVaccineComboByIdAsync(int id)
        {
            return await vaccineComboRepository.GetVaccineComboByIdAsync(id);
        }

        public async Task<List<VaccineCombo>> GetVaccineCombosAsync()
        {
            return await vaccineComboRepository.GetVaccineCombosAsync();
        }


    }
}
