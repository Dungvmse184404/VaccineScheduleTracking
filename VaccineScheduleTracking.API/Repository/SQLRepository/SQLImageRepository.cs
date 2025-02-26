using Microsoft.EntityFrameworkCore;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API_Test.Repository.IRepository;

namespace VaccineScheduleTracking.API_Test.Repository.SQLRepository
{
    public class SQLImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly VaccineScheduleDbContext dbContext;

        public SQLImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, VaccineScheduleDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", image.File.FileName);

            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.File.FileName}";
            image.FilePath = urlFilePath;

            await dbContext.Images.AddAsync(image);
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"EF Core Error: {ex.InnerException?.Message}");
                throw;
            }

            return image;
        }
    }
}
