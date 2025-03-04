using System.ComponentModel.DataAnnotations;

namespace VaccineScheduleTracking.API_Test.Models.DTOs
{
    public class CreateVaccineRecordDto
    {
        [Required]
        public int ChildID { get; set; }
        [Required]
        public int VaccineTypeID { get; set; }
        [Required]
        public int VaccineID { get; set; }
        [Required]
        public int AppointmentID { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public string? Note { get; set; }
    }
}
