using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VaccineScheduleTracking.API_Test.Models.DTOs;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API_Test.Repository.IRepository;
using VaccineScheduleTracking.API_Test.Services;

namespace VaccineScheduleTracking.API_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService imageService;

        public ImageController(IImageService imageService)
        {
            this.imageService = imageService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadDto imageUploadDto)
        {
            try
            {
                var image = await imageService.Upload(imageUploadDto);
                return Ok(new
                {
                    ImagePath = image.FilePath
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new {Message = ex.Message});
            }
        }
    }
}
