using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Models.DTOs
{
    public class VaccineContainerDto
    {
        public int VaccineContainerID { get; set; }
        public VaccineType VaccineType { get; set; }
        public Vaccine Vaccine { get; set; }
    }
}
