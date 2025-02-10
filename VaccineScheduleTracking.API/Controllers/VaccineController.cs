using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API.Services;
using VaccineScheduleTracking.API.Helpers;
using static VaccineScheduleTracking.API.Helpers.MessageColorUtility;

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
        public async Task<IActionResult> GetVaccines([FromQuery] FilterVaccineDto filterVaccineDto)
        {
            var vaccines = await vaccineService.GetVaccinesAsync(filterVaccineDto);

            return Ok(mapper.Map<List<VaccineDto>>(vaccines));
        }

        [Authorize(Roles = "Doctor, Staff")]
        [HttpPost("add-vaccinetype")]
        public async Task<IActionResult> CreateVaccineType([FromBody] AddVaccineTypeDto addVaccineTypeDto)
        {
            var vaccineType = await vaccineService.CreateVaccineTypeAsync(addVaccineTypeDto);

            if (vaccineType == null)
            {
                return BadRequest($"{addVaccineTypeDto.Name} is exist!");
            }

            return Ok(vaccineType);
        }

        [Authorize(Roles = "Doctor, Staff")]
        [HttpPost("add-vaccine")]
        public async Task<IActionResult> CreateVaccine([FromBody] AddVaccineDto addVaccineDto)
        {
            try
            {
                var vaccine = await vaccineService.CreateVaccineAsync(addVaccineDto);
                return Ok(mapper.Map<VaccineDto>(vaccine));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //[Authorize(Roles = "Doctor")]
        [HttpPost("update-vaccine/{id}")]
        public async Task<IActionResult> UpdateVaccine([FromRoute]int id, [FromBody] UpdateVaccineDto updateVaccineDto)
        {
            try
            {
                var vaccine = await vaccineService.UpdateVaccineAsync(id, updateVaccineDto);
                var vaccineDto = mapper.Map<VaccineDto>(vaccine);
                return Ok($"Updated vaccine {vaccineDto.VaccineID} successfully");
            }
            catch (Exception ex)
            {
                DisplayMsg(ex.Message, Color.Red);
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("delete-vaccine")]
        public async Task<IActionResult> DeleteVaccine(int id)
        {
            try
            {
                var vaccine = await vaccineService.DeleteVaccineAsync(id);
                var vaccineDto = mapper.Map<VaccineDto>(vaccine);
                return Ok($"Vaccine {vaccineDto.Name} Deleted Successfully");
            }
            catch (Exception e){ return BadRequest(e.Message); }
        }
    }

}