using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Models.DTOs
{
    public class CreateVaccineContainerDto
    {
        [Required]
        public int VaccineComboID { get; set; }
        [Required]
        public int VaccineID { get; set; }
    }
}
