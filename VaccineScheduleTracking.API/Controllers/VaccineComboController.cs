using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VaccineScheduleTracking.API_Test.Models.DTOs;
using VaccineScheduleTracking.API_Test.Services.VaccinePackage;

namespace VaccineScheduleTracking.API_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineComboController : ControllerBase
    {
        private readonly IVaccineComboService vaccineComboService;
        private readonly IMapper mapper;

        public VaccineComboController(IVaccineComboService vaccineComboService, IMapper mapper)
        {
            this.vaccineComboService = vaccineComboService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetVaccineCombos()
        {
            var vaccineCombos = await vaccineComboService.GetVaccineCombosAsync();

            return Ok(mapper.Map<List<VaccineComboDto>>(vaccineCombos));
        }
    }
}
