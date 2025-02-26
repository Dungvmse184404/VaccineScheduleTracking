using System.ComponentModel.DataAnnotations;

namespace VaccineScheduleTracking.API_Test.Models.DTOs
{
    public class ImageUploadDto
    {
        public IFormFile? File { get; set; }
    }
}
