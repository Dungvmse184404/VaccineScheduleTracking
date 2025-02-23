using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository.DailyTimeSlots
{
    public interface IDailyScheduleRepository
    {
        Task AddDailyScheduleAsync(DailySchedule daily);
        Task<List<DailySchedule>> GetAllDailyScheduleAsync();
        Task<DailySchedule> GetDailyScheduleByDateAsync(DateOnly date);
        //Task<DailySchedule?> GetSlotByID(int SlotID);
        //Task<List<DailySchedule>> GetAllSlotAsync(DateTime date);
        //Task<List<DailySchedule>> GetAvailableSlotsAsync(DateTime date);
        //Task<bool> BookSlotAsync(int slot);
        
    }
}
