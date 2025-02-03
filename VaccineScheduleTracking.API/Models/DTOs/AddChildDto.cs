using System.ComponentModel.DataAnnotations;

namespace VaccineScheduleTracking.API.Models.DTOs
{
    public class AddChildDto
    {
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public decimal Weight { get; set; }
        [Required]
        public decimal Height { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateOnly DateOfBirth { get; set; }
    }
}
