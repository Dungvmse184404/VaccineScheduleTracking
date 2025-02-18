using Microsoft.AspNetCore.Identity;
using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository
{
    public interface ITimeSlotRepository
    {
        Task<TimeSlot?> ChangeTimeSlotStatus(int timeSlotID, bool status);
        Task<TimeSlot?> GetTimeSlotAsync(int timeSlot, DateOnly date);

        //Task<List<TimeSlot>> GetAllAvailableTimeSlotAsync(int id);
    }
}
