using VaccineScheduleTracking.API_Test.Models.DTOs;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API_Test.Repository.IRepository;

namespace VaccineScheduleTracking.API_Test.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository imageRepository;

        public ImageService(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        public async Task<Image> Upload(ImageUploadDto imageUpload)
        {
            var allowExtension = new string[] { ".jpg", "jped", "png" };
            if (!allowExtension.Contains(Path.GetExtension(imageUpload.File.FileName)))
            {
                throw new Exception("Unsupported file extenstion");
            }
            if (imageUpload.File.Length > 10485760)
            {
                throw new Exception("File size is more than 10MB");
            }

            var image = new Image()
            {
                File = imageUpload.File,
                FileExtension = Path.GetExtension(imageUpload.File.FileName),
                FileSize = imageUpload.File.Length
            };

            await imageRepository.Upload(image);
            return image;
        }
    }
}
