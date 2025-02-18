using System.ComponentModel.DataAnnotations;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Models.DTOs.Vaccines
{
    public class AddVaccineDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Manufacturer { get; set; }
        [Required]
        public int Stock { get; set; } = 0;
        [Required]
        public decimal Price { get; set; } = decimal.Zero;
        [Required]
        public string Description { get; set; }
        [Required]
        public int FromAge { get; set; }
        [Required]
        public int ToAge { get; set; }
        [Required]
        public int Period { get; set; }
        [Required]
        public string VaccineType { get; set; }
    }
}
