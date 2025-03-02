namespace VaccineScheduleTracking.API_Test.Models.Entities
{
    public class VaccineCombo
    {
        public int VaccineComboID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<VaccineContainer> VaccineContainers { get; set; } = new();
    }
}
