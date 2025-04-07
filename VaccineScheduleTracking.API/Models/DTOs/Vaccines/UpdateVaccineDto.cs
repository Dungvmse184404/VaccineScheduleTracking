using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Models.DTOs.Vaccines
{
    public class UpdateVaccineDto
    {
        public string Name { get; set; }
        //public int? VaccineTypeID { get; set; } //update VaccineTypeID gây conflict
        public string Manufacturer { get; set; }
        public int? Stock { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int FromAge { get; set; }
        public int ToAge { get; set; }
        public int Period { get; set; }
        public int DosesRequired { get; set; }
        public int Priority { get; set; }

    }
}
