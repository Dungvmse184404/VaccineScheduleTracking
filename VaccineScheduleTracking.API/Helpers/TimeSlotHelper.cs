using VaccineScheduleTracking.API_Test.Configurations;

namespace VaccineScheduleTracking.API_Test.Helpers
{
    public class TimeSlotHelper
    {
        private readonly DefaultConfig _config;

        public TimeSlotHelper(DefaultConfig config)
        {
            _config = config;
        }

        public List<int> AllocateTimeSlots(string? timeSlot)
        {
            return !string.IsNullOrEmpty(timeSlot)
                ? timeSlot.Split(',').Select(int.Parse).ToList()
                : Enumerable.Range(1, 20).ToList();
        }

        public bool ExcludedDay(DateOnly date) => date.DayOfWeek != DayOfWeek.Sunday;

        public int CompareNowTime<T>(T value) where T : struct
        {
            return value switch
            {
                DateTime dt => dt.CompareTo(DateTime.Now),
                TimeOnly to => to.CompareTo(TimeOnly.FromDateTime(DateTime.Now)),
                DateOnly d => d.CompareTo(DateOnly.FromDateTime(DateTime.Now)),
                _ => throw new ArgumentException($"Kiểu {typeof(T)} không được hỗ trợ")
            };
        }

        public int CalculateSlotNumber(TimeOnly startTime)
            => ((startTime.Hour - 7) * 60 + startTime.Minute) / _config.SlotDuration + 1;

        public TimeOnly CalculateStartTime(int slotNumber)
            => new TimeOnly(7, 0).AddMinutes((slotNumber - 1) * _config.SlotDuration);

        public DateOnly CalculateDate(int days)
            => DateOnly.FromDateTime(DateTime.Now).AddDays(days);

        public int SetCalendarDate() =>  _config.ScheduleLength;

        public string ConvertToWeekday(DateOnly date)
            => date.DayOfWeek.ToString();

        public void LimitDate(DateOnly date, string exceptionMsg)
        {
            DateOnly limitDate = CalculateDate(SetCalendarDate());
            if (date >= limitDate)
            {
                throw new Exception($"{exceptionMsg} {limitDate.AddDays(-1)}");
            }
        }

        public DateOnly GetPeriodDate(int weeks, DateOnly date)
            => date.AddDays(weeks * _config.PeriodForVaccine);
    }
}
