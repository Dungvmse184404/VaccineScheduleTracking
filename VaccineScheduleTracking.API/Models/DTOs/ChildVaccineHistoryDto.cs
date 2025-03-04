using System.ComponentModel.DataAnnotations;

namespace VaccineScheduleTracking.API_Test.Models.DTOs
{
    public class ChildVaccineHistoryDto
    {
        [Required]
        public int ChildID { get; set; }
        [Required]
        public int VaccineTypeID { get; set; }
        public int? VaccineID { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public string? Note { get; set; }
    }
}
