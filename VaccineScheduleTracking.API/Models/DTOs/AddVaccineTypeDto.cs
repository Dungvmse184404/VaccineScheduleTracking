using System.ComponentModel.DataAnnotations;

namespace VaccineScheduleTracking.API.Models.DTOs
{
    public class AddVaccineTypeDto
    {
        [Required]
        public string Name { get; set; }
    }
}
