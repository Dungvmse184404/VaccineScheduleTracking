namespace VaccineScheduleTracking.API.Models.Entities
{
    public class Doctor
    {
        public int DoctorID { get; set; }
        public int AccountID { get; set; }
        public Account Account { get; set; }
    }
}
