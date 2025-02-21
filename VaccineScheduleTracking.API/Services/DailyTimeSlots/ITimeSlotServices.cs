
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Services.DailyTimeSlots
{
    public interface ITimeSlotServices
    {
        //Task GenerateTimeSlotsAsync(int dailyScheduleID);
        Task GenerateCalanderAsync(int numberOfDays);
    }
}
