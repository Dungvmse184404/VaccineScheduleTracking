namespace VaccineScheduleTracking.API_Test.Models.DTOs.Appointments
{
    public class UpdateAppointmentDto
    {
        public int AppointmentID { get; set; }
        public int ChildID { get; set; }
        public int DoctorID { get; set; }
        public int VaccineID { get; set; }
        public int TimeSlotID { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}
