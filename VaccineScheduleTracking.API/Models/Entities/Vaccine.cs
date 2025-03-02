using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API.Models.Entities
{
    public class Vaccine
    {
        public int VaccineID { get; set; }
        public string Name { get; set; }
        public int VaccineTypeID { get; set; }
        public string Manufacturer { get; set; }
        public int Stock {  get; set; }
        public decimal Price { get; set; }  
        public string Description { get; set; }
        public int FromAge { get; set; }
        public int ToAge { get; set; }
        public int Period { get; set; }
        public int DosesRequired { get; set; }
        public int Priority { get; set; }
        public VaccineType VaccineType { get; set; }
    }
}
