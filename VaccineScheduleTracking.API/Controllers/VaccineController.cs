using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Services;

namespace VaccineScheduleTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineController : ControllerBase
    {
        private readonly IVaccineService vaccineService;
        private readonly IMapper mapper;

        public VaccineController(IVaccineService vaccineService, IMapper mapper)
        {
            this.vaccineService = vaccineService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] FilterVaccineDto filterVaccineDto)
        {
            var vaccines = await vaccineService.GetVaccinesAsync(filterVaccineDto);

            return Ok(mapper.Map<List<VaccineDto>>(vaccines));
        }
    }
}
