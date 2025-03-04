namespace VaccineScheduleTracking.API_Test.Helpers
{
    public static class TimeSlotHelper
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

        /// <summary>
        /// hàm này so sánh 3 kiểu dữ liệu DateOnly, TimeOnly, DateTime với thời gian hiện tại (BETA)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns> base to type DateOnly||TimeOnly||DateTime </returns>
        /// <exception cref="ArgumentException"></exception>
        public static int CompareNowTime<T>(T value) where T : struct
        {
            return value switch
            {
                DateTime dateTime => dateTime.CompareTo(DateTime.Now),
                TimeOnly timeOnly => timeOnly.CompareTo(TimeOnly.FromDateTime(DateTime.Now)),
                DateOnly dateOnly => dateOnly.CompareTo(DateOnly.FromDateTime(DateTime.Now)) switch
                {
                    0 => 0,
                    var result => result
                },
                _ => throw new ArgumentException("(BE)err: Định dạng ngày truyền vào không hỗ trợ")
            };
        }

        /// <summary>
        /// hàm này dùng cho các timeslot không có startTime
        /// </summary>
        /// <param name="slotNumber"></param>
        /// <returns></returns>
        public static TimeOnly CalculateStartTime(int slotNumber)
            => new TimeOnly(7, 0).AddMinutes((slotNumber - 1) * 45);


        /// <summary>
        /// tính ra những timeSlot đươc tạo đến ngày nào
        /// </summary>
        /// <param name="NumberOfDate"></param>
        /// <returns></returns>
        public static DateOnly CaculateDate(int NumberOfDate)
            => DateOnly.FromDateTime(DateTime.Now).AddDays(NumberOfDate);


        /// <summary>
        /// Set số ngày tạo timeSlot
        /// </summary>
        /// <returns></returns>
        public static int SetCalanderDate() => 7;


        /// <summary>
        /// chuyển từ dạng DateOnly qua DayOfWeek
        /// </summary>
        /// <param name="date"></param>
        /// <returns>return type: enum </returns>
        public static string ConvertToWeekday(DateOnly date)
            => date.DayOfWeek.ToString();


        /// <summary>
        /// hàm này để catch lỗi đặt lịch vào ngày chưa được tạo 
        /// </summary>
        /// <param name="date"></param>
        /// <exception cref="Exception"></exception>
        public static void LimitDate(DateOnly date, string exceptionMsg)
        {
            DateOnly limitDate = CaculateDate(SetCalanderDate());
            if (date >= limitDate)
            {
                throw new Exception($"err: {exceptionMsg} {limitDate.AddDays(-1)}");
            }

        }

    }
}
