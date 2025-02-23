using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Vaccines;

namespace VaccineScheduleTracking.API_Test.Repository.Vaccines
{
    public class SQLVaccineRepository : IVaccineRepository
    {
        private readonly VaccineScheduleDbContext dbContext;

        public SQLVaccineRepository(VaccineScheduleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Vaccine?> GetVaccineByIDAsync(int id)
        {
            return await dbContext.Vaccines.FirstOrDefaultAsync(x => x.VaccineID == id);
        }

        public async Task<Vaccine?> GetVaccineByNameAsync(string name)
        {
            return await dbContext.Vaccines.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<List<Vaccine>> GetVaccineByTypeIDAsync(int typeID)
        {
            return await dbContext.Vaccines.Where(x => x.VaccineTypeID == typeID).ToListAsync();
        }

        public async Task<Vaccine> AddVaccineAsync(Vaccine vaccine)
        {
            await dbContext.Vaccines.AddAsync(vaccine);
            await dbContext.SaveChangesAsync();

            return vaccine;
        }

        /// <summary>
        /// hàm này dùng để tìm những loại vaccine phù hợp cho child dựa trên tuổi
        /// </summary>
        /// <param name="Age"> tuổi của child </param>
        /// <param name="TypeName"> TypeName của vaccine </param>
        /// <returns> Danh sách vaccine phù hợp </returns>
        //public async Task<Vaccine> GetSutableVaccineAsync(int Age, string TypeName)
        //{
        //    var vaccine = dbContext.Vaccines.Where(v => v.FromAge <= Age && v.ToAge >= Age && v.Stock > 0);
        //    return await vaccine.OrderBy(v => v.FromAge).FirstOrDefaultAsync();
        //}


        // Vaccine function
        public async Task<List<Vaccine>> GetVaccinesAsync(FilterVaccineDto filterVaccineDto)
        {
            var query = dbContext.Vaccines.Include(x => x.VaccineType).AsQueryable();

            if (!string.IsNullOrEmpty(filterVaccineDto.Name))
            {
                query = query.Where(x => x.Name.Contains(filterVaccineDto.Name));
            }
            if (!string.IsNullOrEmpty(filterVaccineDto.Manufacturer))
            {
                query = query.Where(x => x.Manufacturer.Contains(filterVaccineDto.Manufacturer));
            }
            if (filterVaccineDto.FromAge.HasValue)
            {
                query = query.Where(x => x.FromAge <= filterVaccineDto.FromAge.Value);
            }
            if (!string.IsNullOrEmpty(filterVaccineDto.VaccineType))
            {
                query = query.Where(x => x.VaccineType.Name.Contains(filterVaccineDto.VaccineType));
            }

            return await query.ToListAsync();
        }

        public async Task<Vaccine?> DeleteVaccineAsync(Vaccine deleteVaccine)
        {
            dbContext.Vaccines.Remove(deleteVaccine);
            await dbContext.SaveChangesAsync();

            return deleteVaccine;
        }

        public async Task<Vaccine?> UpdateVaccineAsync(Vaccine UpdateVaccine)
        {
            var vaccine = await GetVaccineByIDAsync(UpdateVaccine.VaccineID);
            if (vaccine == null) { return null; }

            vaccine.Name = UpdateVaccine.Name;
            vaccine.VaccineTypeID = UpdateVaccine.VaccineTypeID;
            vaccine.Manufacturer = UpdateVaccine.Manufacturer;
            vaccine.Stock = UpdateVaccine.Stock;
            vaccine.Price = UpdateVaccine.Price;
            vaccine.Description = UpdateVaccine.Description;
            vaccine.FromAge = UpdateVaccine.FromAge;
            vaccine.ToAge = UpdateVaccine.ToAge;
            vaccine.Period = UpdateVaccine.Period;
            vaccine.DosesRequired = UpdateVaccine.DosesRequired;
            vaccine.Priority = UpdateVaccine.Priority;

            await dbContext.SaveChangesAsync();

            return vaccine;
        }



        // VaccineType function
        public async Task<VaccineType?> GetVaccineTypeByNameAsync(string name)
        {
            return await dbContext.VaccineTypes.FirstOrDefaultAsync(x => x.Name == name);
        }
        public async Task<VaccineType> GetVaccineTypeByIDAsync(int id)
        {
            return await dbContext.VaccineTypes.FirstOrDefaultAsync(x => x.VaccineTypeID == id);
        }

        public async Task<VaccineType> AddVaccineTypeAsync(VaccineType vaccineType)
        {
            await dbContext.VaccineTypes.AddAsync(vaccineType);
            await dbContext.SaveChangesAsync();

            return vaccineType;
        }

        public async Task<VaccineType> UpdateVaccineTypeAsync(VaccineType vaccineType)
        {
            var Type = await GetVaccineTypeByIDAsync(vaccineType.VaccineTypeID);
            if (Type == null)
            {
                return null;
            }
            Type.Name = vaccineType.Name;
            Type.Description = vaccineType.Description;

            await dbContext.SaveChangesAsync();
            return Type;
        }


        public async Task<VaccineType> DeleteVaccineTypeAsync(VaccineType vaccineType)
        {
            dbContext.VaccineTypes.Remove(vaccineType);
            await dbContext.SaveChangesAsync();

            return vaccineType;
        }

        public async Task<List<VaccineType>> GetVaccinesTypeAsync()
        {
            return await dbContext.VaccineTypes.ToListAsync();
        }


    }
}