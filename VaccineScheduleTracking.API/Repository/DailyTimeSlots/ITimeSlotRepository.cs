using Microsoft.AspNetCore.Identity;
using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository.DailyTimeSlots
{
    public interface ITimeSlotRepository
    {
        Task<TimeSlot?> GetTimeSlotByIDAsync(int id);
        Task<TimeSlot?> ChangeTimeSlotStatus(int timeSlotID, bool status);
        Task<TimeSlot?> GetTimeSlotAsync(int timeSlot, DateOnly date);
        Task AddTimeSlotForDayAsync(TimeSlot timeSlots);
        //Task<List<TimeSlot>> GetAllAvailableTimeSlotAsync(int id);
    }
}
