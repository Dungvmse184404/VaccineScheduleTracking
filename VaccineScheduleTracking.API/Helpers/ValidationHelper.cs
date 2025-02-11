namespace VaccineScheduleTracking.API.Helpers
{
    public class ValidationHelper
    {
        public static bool NullValidator<T>(T value)
        {
            if (value == null) return false;
            if (value is string str && str == "string") return false;
            if (value is int num && num == 0) return false;
            if (value is double dnum && dnum == 0) return false;
            return true;
        }
    }
}
