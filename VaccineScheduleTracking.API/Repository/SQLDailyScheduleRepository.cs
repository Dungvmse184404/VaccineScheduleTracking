using Microsoft.EntityFrameworkCore;
using System;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Repository
{
    public class SQLDailyScheduleRepository : IDailyScheduleRepository
    {
        private readonly VaccineScheduleDbContext _dbContext;

        public SQLDailyScheduleRepository(VaccineScheduleDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<bool> BookSlotAsync(int slotID, int appointmentID)
        {
            var Slot = await GetSlotByID(slotID);
            if (Slot.AppointmentID != null || Slot == null)
            {
                return false;
            }

            Slot.AppointmentID = appointmentID;
            return await _dbContext.SaveChangesAsync() > 0;
        }
        public async Task<DailySchedule?> GetSlotByID(int SlotID)
        {
            return await _dbContext.DailySchedule.FirstOrDefaultAsync(s => s.Slot == SlotID);
        }
        public async Task<List<DailySchedule>> GetAllSlotAsync(DateTime date)
        {
            return await _dbContext.DailySchedule.Where(s => s.Appointment.Time == date).ToListAsync();
        }

        public async Task<List<DailySchedule>> GetAvailableSlotsAsync(DateTime date)
        {
            return await _dbContext.DailySchedule.Where(s => s.Appointment.Time == date && s.AppointmentID == null).ToListAsync();
        }


        /// <summary>
        /// hàm này tạo 20 slot cho 1 ngày
        /// </summary>`
        /// <param name="date"> là ngày muốn tạo slot </param>
        /// <returns> (DB) </returns>
        //public async Task GenerateSlotsForDay(DateTime date)
        //{
        //    var startTime = new TimeSpan(7, 0, 0); // 07:00
        //    var slotDuration = new TimeSpan(0, 45, 0); // 45'/slot
        //    int totalSlots = 20; // Số slot mỗi ngày

        //    for (int i = 0; i < totalSlots; i++)
        //    {
        //        var slotTime = startTime.Add(TimeSpan.FromMinutes(i * 45));

        //        var existingSlot = await _dbContext.Slots.FirstOrDefaultAsync(s => s.startTime == slotTime && s.AppointmentDate == date);

        //        if (existingSlot == null)
        //        {
        //            _dbContext.Slots.Add(new Slot
        //            {
        //                startTime = slotTime,
        //                AppointmentDate = date
        //            });
        //        }
        //    }
        //    await _dbContext.SaveChangesAsync();
        //}


        /// <summary>
        /// hàm này tạo ra lịch để người dùng đặt Slot
        /// </summary>
        /// <param name="month"> độ dài lịch tính từ thời điểm hiện tại </param>
        /// <returns> (DB) tạo ta lịch có độ dài tương ứng month </returns>
        //public async Task GenerateSlotsForMonth(int month)
        //{
        //    var currentDate = DateTime.Now;
        //    var endDate = currentDate.AddMonths(month);

        //    while (currentDate <= endDate)
        //    {
        //        await GenerateSlotsForDay(currentDate);
        //        currentDate = currentDate.AddDays(1);
        //    }
        //}

    }
}
