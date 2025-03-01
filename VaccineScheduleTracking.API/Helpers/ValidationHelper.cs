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

        public static void ValidateInput<T>(T input, string errorMessage)
        {
            if (EqualityComparer<T>.Default.Equals(input, default))
            {
                throw new ArgumentException(errorMessage);
            }
        }
    }
}
