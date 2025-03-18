using System.ComponentModel.DataAnnotations;

namespace VaccineScheduleTracking.API_Test.Models.DTOs.Doctors
{
    public class PromoteDoctorDto
    {
        [Required]
        public int AccountID { get; set; }
        public string? DoctorSchedule { get; set; }
    }
}
