using System.ComponentModel.DataAnnotations;
using VaccineScheduleTracking.API_Test.Helpers;

namespace VaccineScheduleTracking.API_Test.Models.DTOs.Appointments
{
    public class CreateAppointmentDto
    {
        [Required]
        public int ChildID { get; set; }
        [Required]
        public int VaccineID { get; set; }
        [Required]
        [ValidDate]
        public DateOnly Date { get; set; }
        [Required]
        public int SlotNumber { get; set; }
    }
}
