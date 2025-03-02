using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Models.DTOs
{
    public class VaccineRecordDto
    {
        public int VaccineRecordID { get; set; }
        public DateOnly Date { get; set; }
        public string? Note { get; set; }
        public VaccineType VaccineType { get; set; }
        public Vaccine? Vaccine { get; set; }
        public Appointment? Appointment { get; set; }
    }
}
