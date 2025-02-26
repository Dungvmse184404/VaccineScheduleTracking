using System.ComponentModel.DataAnnotations.Schema;

namespace VaccineScheduleTracking.API_Test.Models.Entities
{
    public class Image
    {
        public int ImageID { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
        public string FileExtension { get; set; }
        public int FileSize { get; set; }
        public string FilePath { get; set; }
    }
}
