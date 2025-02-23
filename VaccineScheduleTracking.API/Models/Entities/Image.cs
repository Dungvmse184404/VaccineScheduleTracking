namespace VaccineScheduleTracking.API_Test.Models.Entities
{
    public class Image
    {
        public int ID { get; set; }
        public IFormFile File { get; set; }
        public string FileExtension { get; set; }
        public long FileSize { get; set; }
        public string FilePath { get; set; }
    }
}
