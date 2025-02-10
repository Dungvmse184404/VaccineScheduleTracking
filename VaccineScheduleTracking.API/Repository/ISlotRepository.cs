using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Repository
{
    public interface ISlotRepository
    {
        Task<Slot?> GetSlotByID(int SlotID);
        Task<List<Slot>> GetAllSlotAsync(DateTime date);
        Task<List<Slot>> GetAvailableSlotsAsync(DateTime date);
        Task<bool> BookSlotAsync(int slot, int appointmentID);
        //Task GenerateSlotsForMonth(int month);
    }
}
