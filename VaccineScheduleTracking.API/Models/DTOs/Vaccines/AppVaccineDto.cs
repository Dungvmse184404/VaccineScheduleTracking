using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Models.DTOs.Vaccines
{
    public class AppVaccineDto
    {
        public int VaccineID { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public VaccineType VaccineType { get; set; }
    }
}
