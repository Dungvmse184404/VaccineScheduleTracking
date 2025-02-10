namespace VaccineScheduleTracking.API.Models.Entities
{
    public class Doctor
    {
        public int DoctorID { get; set; }
        public int AccountID { get; set; }
        public List<int> AvailableSlots { get; set; }
        public Account Account { get; set; }
    }
}
