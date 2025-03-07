using System.Text.Json.Serialization;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Models.Entities
{
    public class VaccineContainer
    {
        public int VaccineContainerID { get; set; }
        public int VaccineComboID { get; set; }
        public int VaccineID { get; set;}

        [JsonIgnore]
        public VaccineCombo VaccineCombo { get; set; }
        public Vaccine Vaccine { get; set; }
    }
}
