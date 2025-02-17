using Microsoft.AspNetCore.Identity;
using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository.IRepository
{
    public interface ITimeSlotRepository
    {
        Task<TimeSlot?> ChangeTimeSlotStatus(int timeSlotID, bool status);
        //Task<bool> CheckSlotAsync(int slot, DateOnly date);
        Task<TimeSlot?> GetTimeSlotAsync(int timeSlot, DateOnly date);

        //Task<List<TimeSlot>> GetAllAvailableTimeSlotAsync(int id);
    }
}
