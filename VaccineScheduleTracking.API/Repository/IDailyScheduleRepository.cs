using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository
{
    public interface IDailyScheduleRepository
    {
        Task AddDailyScheduleAsync(DailySchedule dailySchedule);
        Task<DailySchedule> GetDailyScheduleByDateAsync(DateOnly date);
        Task<DailySchedule?> GetSlotByID(int SlotID);
        Task<List<DailySchedule>> GetAllSlotAsync(DateTime date);
        Task<List<DailySchedule>> GetAvailableSlotsAsync(DateTime date);
        Task<bool> BookSlotAsync(int slot);
    }
}
