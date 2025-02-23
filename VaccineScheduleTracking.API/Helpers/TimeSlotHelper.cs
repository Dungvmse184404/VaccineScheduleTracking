namespace VaccineScheduleTracking.API_Test.Helpers
{
    public class TimeSlotHelper
    {
        public static List<int> AllocateTimeSlotsAsync(string? timeSlot)
        {
            List<int> slotNumbers;

            if (!string.IsNullOrEmpty(timeSlot))
            {
                slotNumbers = timeSlot.Split(',').Select(int.Parse).ToList();
            }
            else
            {
                slotNumbers = Enumerable.Range(1, 20).ToList();
            }

            return slotNumbers;
        }

        public static bool ExcludedDay(DateOnly date) 
            => date.DayOfWeek != DayOfWeek.Sunday;
        


    }
}
