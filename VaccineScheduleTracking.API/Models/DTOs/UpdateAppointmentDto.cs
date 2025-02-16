namespace VaccineScheduleTracking.API.Models.DTOs
{
    public class UpdateAppointmentDto
    {
        public int AppointmentID { get; set; }
        public int ChildID { get; set; }
        public int DoctorID { get; set; }
        public int VaccineTypeID { get; set; }
        public int Slot { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}
