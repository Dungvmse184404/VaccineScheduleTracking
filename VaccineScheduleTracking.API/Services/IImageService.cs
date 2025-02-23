using VaccineScheduleTracking.API_Test.Models.DTOs;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API_Test.Repository.IRepository;

namespace VaccineScheduleTracking.API_Test.Services
{
    public interface IImageService
    {
        Task<Image> Upload(ImageUploadDto imageUpload);
    }
}
