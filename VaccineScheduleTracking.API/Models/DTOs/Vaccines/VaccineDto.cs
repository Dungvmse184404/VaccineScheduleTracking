using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Models.DTOs.Vaccines
{
    public class VaccineDto
    {
        public int VaccineID { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public int Stock { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public int FromAge { get; set; }
        public int ToAge { get; set; }
        public int Period { get; set; }
        public int DosesRequired { get; set; }
        public int Priority { get; set; }
        public VaccineType VaccineType { get; set; }
    }
}
