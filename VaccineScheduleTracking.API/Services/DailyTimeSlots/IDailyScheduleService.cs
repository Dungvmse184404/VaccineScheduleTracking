
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Services.DailyTimeSlots
{
    public interface IDailyScheduleService
    {
        Task<DailySchedule?> GetDailyScheduleByIDAsync(int dailyScheduleID);
        Task<DailySchedule?> GetDailyScheduleByDateAsync(DateOnly date);
    }
}
