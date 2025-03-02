using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Models.DTOs
{
    public class VaccineComboDto
    {
        public int VaccineComboID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<VaccineContainerDto> VaccineContainers { get; set; } = new();
    }
}
