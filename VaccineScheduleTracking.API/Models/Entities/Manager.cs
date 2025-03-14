using System.Text.Json.Serialization;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Models.Entities
{
    public class Manager
    {
        public int ManagerID { get; set; }
        public int AccountID { get; set; }
        [JsonIgnore]
        public Account Account { get; set; }
    }
}
