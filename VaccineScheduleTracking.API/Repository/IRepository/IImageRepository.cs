using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository.IRepository
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
