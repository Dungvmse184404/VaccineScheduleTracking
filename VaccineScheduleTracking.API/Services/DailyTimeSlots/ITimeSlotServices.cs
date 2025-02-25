
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Services.DailyTimeSlots
{
    public interface ITimeSlotServices
    {
        //Task GenerateTimeSlotsAsync(int dailyScheduleID);
        Task GenerateCalanderAsync(int numberOfDays);
        Task<TimeSlot> UpdateTimeSlotAsync(TimeSlot timeSlot);
        Task <TimeSlot?> GetTimeSlotAsync(int SlotNumber, DateOnly Date);
        Task SetOverdueTimeSlotAsync();
    }
}
