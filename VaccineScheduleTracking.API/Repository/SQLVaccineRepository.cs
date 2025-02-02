using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Repository
{
    public class SQLVaccineRepository : IVaccineRepository
    {
        private readonly VaccineScheduleDbContext dbContext;

        public SQLVaccineRepository(VaccineScheduleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Vaccine?> GetVaccineByNameAsync(string name)
        {
            return await dbContext.Vaccines.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<Vaccine> AddVaccineAsync(Vaccine vaccine)
        {
            await dbContext.Vaccines.AddAsync(vaccine);
            await dbContext.SaveChangesAsync();

            return vaccine;
        }


        // Vaccine function
        public async Task<List<Vaccine>> GetVaccinesAsync(FilterVaccineDto filterVaccineDto)
        {
            var query = dbContext.Vaccines.Include(x => x.VaccineType).AsQueryable();

            if (!string.IsNullOrEmpty(filterVaccineDto.Name))
            {
                query = query.Where(x => x.Name.Contains(filterVaccineDto.Name));
            }
            if(!string.IsNullOrEmpty(filterVaccineDto.Manufacturer))
            {
                query = query.Where(x => x.Manufacturer.Contains(filterVaccineDto.Manufacturer));
            }
            if(filterVaccineDto.FromAge.HasValue)
            {
                query = query.Where(x => x.FromAge <= filterVaccineDto.FromAge.Value);
            }
            if (!string.IsNullOrEmpty(filterVaccineDto.VaccineType))
            {
                query = query.Where(x => x.VaccineType.Name.Contains(filterVaccineDto.VaccineType));
            }

            return await query.ToListAsync();
        }


        // VaccineType function
        public async Task<VaccineType?> GetVaccineTypeByNameAsync(string name)
        {
            return await dbContext.VaccineTypes.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<VaccineType> AddVaccineTypeAsync(VaccineType vaccineType)
        {
            await dbContext.VaccineTypes.AddAsync(vaccineType);
            await dbContext.SaveChangesAsync();

            return vaccineType;
        }

        
    }
}
