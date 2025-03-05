using System.ComponentModel.DataAnnotations;

namespace VaccineScheduleTracking.API_Test.Models.DTOs
{
    public class DeleteVaccineContainerDto
    {
        [Required]
        public int VaccineComboID { get; set; }
        [Required]
        public int VaccineContainerID { get; set; }
    }
}
