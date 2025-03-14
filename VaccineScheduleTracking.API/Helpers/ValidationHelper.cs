namespace VaccineScheduleTracking.API_Test.Helpers
{
    public static class ValidationHelper
    {

        public static bool NullValidator<T>(T value)
        {
            if (value == null) return false;
            if (value is string str && str == "string") return false;
            if (value is int num && num == 0) return false;
            if (value is double dnum && dnum == 0) return false;
            return true;
        }

        public static T ValidateInput<T>(T input, string errorMessage)
        {
            if (input is string str)
            {
                input = (T)(object)str.Trim();
            }
            if (EqualityComparer<T>.Default.Equals(input, default))
            {
                throw new ArgumentException(errorMessage);
            }

           return input;
        }


        /// <summary>
        /// dùng riêng để validate cho appointment.Status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string ValidateStatus(string status)
        {
            string[] validStatuses = { "CONFIRMED", "FINISHED", "CANCELED", "OVERDUE", "PENDING" };
            var s = ValidateInput(status, "status nhập vào không thể để trống").ToUpper();

            if (!validStatuses.Contains(s))
            {
                throw new Exception("Sai format trạng thái (CONFIRMED || PENDING || CONFIRMED || OVERDUE || CANCELED || FINISHED)");
            }

            return s;
        }



        public static string ValidateDoctorSchedule(string doctorSchedule)
        {
            if (string.IsNullOrWhiteSpace(doctorSchedule))
            {
                Console.WriteLine("doctorSchedule is null!! ");
                return doctorSchedule;
            }

            string[] validWeekdays = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            var scheduleParts = doctorSchedule.Split('|');

            foreach (var part in scheduleParts)
            {
                var dayAndSlots = part.Split(':');
                if (dayAndSlots.Length != 2)
                {
                    throw new Exception("inputErr: Sai format DoctorSchedule (Monday:1,2,3...|Tuesday:5,6,7,8...)");
                }

                var day = dayAndSlots[0];
                var slots = dayAndSlots[1].Split(',');

                if (!validWeekdays.Contains(day))
                {
                    throw new Exception($"inputErr: Weekdays không hợp lệ: {day} (Monday:1,2,3...|Tuesday:5,6,7,8...)");

                }


                foreach (var slot in slots)
                {
                    if (!int.TryParse(slot, out _))
                    {
                        throw new Exception($"inputErr: Slot không hợp lệ: {slot} (Monday:1,2,3...|Tuesday:5,6,7,8...)");
                    }
                }
            }
            return doctorSchedule;
        }


        public static string ValidateWeekday(string weekday)
        {
            string[] validWeekdays = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            var wd = ValidateInput(weekday, "weekday nhập vào không thể để trống");

            if (!validWeekdays.Contains(wd))
            {
                throw new Exception("DataErr: Sai format của DoctorSchedule (Monday:1,2,3...|Tuesday:5,6,7,8...)");
            }

            return wd;
        }
    }
}
