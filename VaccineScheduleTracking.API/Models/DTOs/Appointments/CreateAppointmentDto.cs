using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel.DataAnnotations;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Models.DTOs.Appointments
{
    public class CreateAppointmentDto
    {
        [Required]
        public int ChildID { get; set; }
        [Required]
        public int VaccineID { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public int SlotNumber { get; set; }
    }
}
