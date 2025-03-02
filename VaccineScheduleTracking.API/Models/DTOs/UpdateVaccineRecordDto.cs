using System.ComponentModel.DataAnnotations;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Models.DTOs
{
    public class UpdateVaccineRecordDto
    {
        [Required]
        public int VaccineRecordID { get; set; }
        [Required]
        public int VaccineTypeID { get; set; }
        public int? VaccineID { get; set; }
        public int? AppointmentID { get; set; }
        public DateOnly Date { get; set; }
        public string? Note { get; set; }
    }
}
