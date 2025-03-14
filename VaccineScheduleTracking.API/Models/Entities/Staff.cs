using System.Text.Json.Serialization;

namespace VaccineScheduleTracking.API.Models.Entities
{
    public class Staff
    {
        public int StaffID { get; set; }
        public int AccountID { get; set; }
        [JsonIgnore]
        public Account Account { get; set; }
    }
}
