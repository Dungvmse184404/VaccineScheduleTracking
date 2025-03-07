namespace VaccineScheduleTracking.API_Test.Configurations
{
    public class DefaultConfig
    {
        // để hết trong appsetting rồi nha
        public int SlotDuration { get; set; } // Độ dài mỗi slot (phút)
        public int ScheduleLength { get; set; } // Độ dài lịch hẹn (ngày)
        public int PeriodForVaccine { get; set; } // Chu kỳ tiêm vaccine (ngày)


    }
}
