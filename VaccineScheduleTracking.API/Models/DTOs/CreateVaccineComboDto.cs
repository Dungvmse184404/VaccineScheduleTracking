using System.ComponentModel.DataAnnotations;

namespace VaccineScheduleTracking.API_Test.Models.DTOs
{
    public class CreateVaccineComboDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
