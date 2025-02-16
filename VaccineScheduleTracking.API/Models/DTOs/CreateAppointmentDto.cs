using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel.DataAnnotations;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Models.DTOs
{
    public class CreateAppointmentDto
    {
        [Required]
        public int ChildID { get; set; }
        [Required]
        public int VaccineName { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public int TimeSlot { get; set; }
    }
}
