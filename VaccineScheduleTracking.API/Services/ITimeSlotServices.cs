namespace VaccineScheduleTracking.API_Test.Services
{
    public interface ITimeSlotServices
    {

        //Task<bool> CheckSlotAsync(int slot, DateOnly Date);
        Task GenerateTimeSlotsForDaysAsync(int numberOfDays);
    }
}
