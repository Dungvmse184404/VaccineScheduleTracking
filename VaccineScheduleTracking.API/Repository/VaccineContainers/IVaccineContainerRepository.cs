using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository.VaccineContainers
{
    public interface IVaccineContainerRepository
    {
        Task<VaccineContainer> AddVaccineContainer(VaccineContainer vaccineContainer);
        Task<VaccineContainer?> GetVaccineContainerByIdAsync(int id);
        Task<VaccineContainer?> DeleteVaccineContainerAsync(int id);
    }
}
