using Microsoft.EntityFrameworkCore;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository
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

        public Task GenerateTimeSlotsForDaysAsync(int numberOfDays)// hàm này để hết lõi thôi
        {
            throw new NotImplementedException();
        }

        /// hàm này tạo timeslot thôi ko trả về gì cả
        /// </summary>
        /// <param name="date"> dd-MM-yyyy </param>
        /// <returns></returns>
        //public async Task GenerateDailyTimeSlotsAsync(DateOnly date)
        //{
        //    var daily = await _dbContext.DailySchedule.FirstOrDefaultAsync(t => t.AppointmentDate == date);

        //    if (daily != null) return;

        //    daily = new DailySchedule
        //    {
        //        AppointmentDate = date,
        //        //TimeSlots = new List<TimeSlot>()
        //    };


        //    List<TimeSlot> slots = new();

        //    for (int slotNumber = 1; slotNumber <= 20; slotNumber++)
        //    {
        //        TimeOnly startTime = new TimeOnly(7, 0).AddMinutes((slotNumber - 1) * 45);

        //        slots.Add(new TimeSlot
        //        {
        //            StartTime = startTime,
        //            SlotNumber = slotNumber,
        //            Available = true,
        //            DailyScheduleID = daily.DailyScheduleID
        //        });
        //    }

        //    _dbContext.TimeSlots.AddRange(slots);
        //    await _dbContext.SaveChangesAsync();

        //}


        /// <summary>
        /// hàm này tạo timeslot cho n ngày tiếp theo       (đù nó tự tạo comment luôn (0_0'))
        /// </summary>
        /// <param name="numberOfDays"></param>
        /// <returns></returns>
        //public async Task GenerateTimeSlotsForDaysAsync(int numberOfDays)
        //{
        //    DateOnly today = DateOnly.FromDateTime(DateTime.Today);

        //    for (int i = 0; i < numberOfDays; i++)
        //    {
        //        DateOnly date = today.AddDays(i);
        //        await GenerateDailyTimeSlotsAsync(date);
        //    }
        //}

    }

}