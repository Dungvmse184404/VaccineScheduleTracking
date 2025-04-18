﻿using Microsoft.EntityFrameworkCore;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository.VaccinePackage
{
    public class SQLVaccineComboRepository : IVaccineComboRepository
    {
        private readonly VaccineScheduleDbContext dbContext;

        public SQLVaccineComboRepository(VaccineScheduleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<VaccineCombo> AddVaccineComboAsync(VaccineCombo vaccineCombo)
        {
            await dbContext.VaccineCombos.AddAsync(vaccineCombo);
            await dbContext.SaveChangesAsync();
            return vaccineCombo;
        }

        public async Task<VaccineCombo?> DeleteVaccineComboAsync(int id)
        {
            var vaccineCombo = await dbContext.VaccineCombos.FirstOrDefaultAsync(x => x.VaccineComboID == id);
            if (vaccineCombo == null)
            {
                return null;
            }
            dbContext.VaccineCombos.Remove(vaccineCombo);
            await dbContext.SaveChangesAsync();
            return vaccineCombo;
        }

        public async Task<VaccineCombo?> GetVaccineComboByIdAsync(int id)
        {
            return await dbContext.VaccineCombos.Include(x => x.VaccineContainers)
                                                .ThenInclude(x => x.Vaccine)
                                                .ThenInclude(x => x.VaccineType)
                                                .FirstOrDefaultAsync(x => x.VaccineComboID == id);
        }

        public async Task<List<VaccineCombo>> GetVaccineCombosAsync()
        {
            return await dbContext.VaccineCombos.Include(x => x.VaccineContainers)
                                                .ThenInclude(x => x.Vaccine)
                                                .ThenInclude(x => x.VaccineType)
                                                .ToListAsync();
        }
    }
}
