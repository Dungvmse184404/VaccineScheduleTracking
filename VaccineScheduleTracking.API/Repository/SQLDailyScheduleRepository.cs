using Microsoft.EntityFrameworkCore;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository
{
    public class SQLDailyScheduleRepository : IDailyScheduleRepository
    {

        private readonly VaccineScheduleDbContext _dbContext;

        public SQLDailyScheduleRepository(VaccineScheduleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> BookSlotAsync(int slot)
        {
            throw new NotImplementedException();
        }

        public Task<List<DailySchedule>> GetAllSlotAsync(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<List<DailySchedule>> GetAvailableSlotsAsync(DateTime date)
        {
            throw new NotImplementedException();
        }

        public async Task<DailySchedule?> GetDailyScheduleByID(int SlotID)
        {
            return await _dbContext.DailySchedule.FirstOrDefaultAsync(x => x.DailyScheduleID == SlotID);
        }

        public async Task<DailySchedule?> GetDailyScheduleByDateAsync(DateOnly date)
        {
            return await _dbContext.DailySchedule.FirstOrDefaultAsync(x => x.AppointmentDate == date);
        }

        public async Task AddDailyScheduleAsync(DailySchedule dailySchedule)
        {
            _dbContext.DailySchedule.Add(dailySchedule);
            await _dbContext.SaveChangesAsync();
        }

        //public async Task<DailySchedule> DeleteDailyScheduleAsync()
        //{
          
        //}

        public Task<DailySchedule?> GetSlotByID(int SlotID)
        {
            throw new NotImplementedException();
        }
    }
}
