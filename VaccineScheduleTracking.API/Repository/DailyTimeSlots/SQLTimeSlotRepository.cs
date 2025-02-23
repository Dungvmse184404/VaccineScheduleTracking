using Microsoft.EntityFrameworkCore;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository.DailyTimeSlots
{

    public class SQLTimeSlotRepository : ITimeSlotRepository
    {
        private readonly VaccineScheduleDbContext _dbContext;

        public SQLTimeSlotRepository(VaccineScheduleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TimeSlot?> GetTimeSlotAsync(int slot, DateOnly date)
        {
            return await _dbContext.TimeSlots.FirstOrDefaultAsync(s => s.SlotNumber == slot && s.DailySchedule.AppointmentDate == date);
        }

        public async Task<TimeSlot?> GetTimeSlotByIDAsync(int id)
        {
            return await _dbContext.TimeSlots.FirstOrDefaultAsync(ts => ts.TimeSlotID == id);
        }

        public async Task<List<TimeSlot>> GetAllAvailebleTimeSlot(int id)
        {
            return await _dbContext.TimeSlots.Where(ts => ts.Available == true).ToListAsync();
        }


        //public async Task<TimeSlot?> GetSlotBySlotNumber(int slotNum)
        //{
        //    return await _dbContext.TimeSlots.FirstOrDefaultAsync(ts => ts.SlotNumber == slotNum);
        //}
        // Function
        public async Task<List<TimeSlot>> GetAllTimeSlot(int id)
        {
            return await _dbContext.TimeSlots.ToListAsync();
        }

        public async Task<TimeSlot?> ChangeTimeSlotStatus(int timeSlotID, bool status)
        {
            var timeSlot = await GetTimeSlotByIDAsync(timeSlotID);

            if (timeSlot == null)
            {
                return null;
            }
            timeSlot.Available = status;
            await _dbContext.SaveChangesAsync();

            return timeSlot;
        }

        public async Task AddTimeSlotForDayAsync(TimeSlot timeSlots)
        {
            _dbContext.TimeSlots.Add(timeSlots);
            await _dbContext.SaveChangesAsync();

        }

        public async Task<TimeSlot> UpdateTimeSlotAsync(TimeSlot timeSlots)
        {
            var slot = await GetTimeSlotByIDAsync(timeSlots.TimeSlotID);
            if (slot == null)
            {
                return null;
            }
            slot.StartTime = timeSlots.StartTime;
            slot.SlotNumber = timeSlots.SlotNumber;
            slot.Available = timeSlots.Available;
            slot.DailyScheduleID = timeSlots.DailyScheduleID;
            await _dbContext.SaveChangesAsync();
            return slot;
        }



    }
}