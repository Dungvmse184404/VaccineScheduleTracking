using Microsoft.EntityFrameworkCore;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository.VaccineContainers
{
    public class SQLVaccineContainerRepository : IVaccineContainerRepository
    {
        private readonly VaccineScheduleDbContext dbContext;

        public SQLVaccineContainerRepository(VaccineScheduleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<VaccineContainer> AddVaccineContainer(VaccineContainer vaccineContainer)
        {
            await dbContext.VaccineContainers.AddAsync(vaccineContainer);
            await dbContext.SaveChangesAsync();
            return vaccineContainer;
        }

        public async Task<VaccineContainer?> DeleteVaccineContainerAsync(int id)
        {
            var vaccineContainer = await GetVaccineContainerByIdAsync(id);
            if (vaccineContainer != null)
            {
                dbContext.VaccineContainers.Remove(vaccineContainer);
                await dbContext.SaveChangesAsync();
            }
            return null;
        }

        public async Task<VaccineContainer?> GetVaccineContainerByIdAsync(int id)
        {
            return await dbContext.VaccineContainers.FirstOrDefaultAsync(x => x.VaccineContainerID == id);
        }
    }
}
