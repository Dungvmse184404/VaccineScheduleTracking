using Microsoft.EntityFrameworkCore;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API.Models.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VaccineScheduleTracking.API_Test.Repository.DailyTimeSlots
{
    public class SQLDailyScheduleRepository : IDailyScheduleRepository
    {

        private readonly VaccineScheduleDbContext _dbContext;

        public SQLDailyScheduleRepository(VaccineScheduleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //public Task<bool> BookSlotAsync(int slot)
        //{
        //    throw new NotImplementedException();
        //}


        public async Task<List<DailySchedule>> GetAllDailyScheduleAsync()
        {
            return await _dbContext.DailySchedule.ToListAsync();
        }
        //public Task<List<DailySchedule>> GetAvailableSlotsAsync(DateTime date)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<DailySchedule?> GetDailyScheduleByID(int SlotID)
        {
            return await _dbContext.DailySchedule.FirstOrDefaultAsync(x => x.DailyScheduleID == SlotID);
        }

        public async Task<DailySchedule?> GetDailyScheduleByDateAsync(DateOnly date)
        {
            return await _dbContext.DailySchedule.FirstOrDefaultAsync(x => x.AppointmentDate == date);
        }
        public async Task<DailySchedule?> GetDailyScheduleByIDAsync(int dailyScheduleID)
        {
            return await _dbContext.DailySchedule.FirstOrDefaultAsync(x => x.DailyScheduleID == dailyScheduleID);
        }



        public async Task AddDailyScheduleAsync(DailySchedule dailySchedule)
        {
            _dbContext.DailySchedule.Add(dailySchedule);
            await _dbContext.SaveChangesAsync();
        }


    }
}
