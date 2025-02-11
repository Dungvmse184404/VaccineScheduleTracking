using System.ComponentModel.DataAnnotations;

namespace VaccineScheduleTracking.API.Models.DTOs
{
    public class DailyScheduleDto
    {
        [Required]
        public int SlotID { get; set; }
        [Required]
        public TimeSpan startTime { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}
