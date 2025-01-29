using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Models.DTOs
{
    public class FilterVaccineDto
    {
        public string? Name { get; set; }
        public string? Manufacturer { get; set; }
        public int? FromAge { get; set; }
        public string? VaccineType { get; set; }
    }
}
