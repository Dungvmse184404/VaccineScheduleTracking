using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Repository
{
    public interface IDailyScheduleRepository
    {
        Task<DailySchedule?> GetSlotByID(int SlotID);
        Task<List<DailySchedule>> GetAllSlotAsync(DateTime date);
        Task<List<DailySchedule>> GetAvailableSlotsAsync(DateTime date);
        Task<bool> BookSlotAsync(int slot);
        //Task GenerateSlotsForMonth(int month);
    }
}
