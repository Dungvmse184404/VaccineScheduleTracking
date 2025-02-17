using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Models.DTOs.Vaccines
{
    public class FilterVaccineDto
    {
        public string? Name { get; set; }
        public string? Manufacturer { get; set; }
        public int? FromAge { get; set; }
        public string? VaccineType { get; set; }
    }
}
