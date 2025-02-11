using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API.Services;
using VaccineScheduleTracking.API.Helpers;
using VaccineScheduleTracking.API_Test.Models.DTOs;

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

    //VaccineType
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
        [HttpPut("Update-vaccinetype/{id}")]
        public async Task<IActionResult> UpdateVaccineType([FromRoute] int id, UpdateVaccineTypeDto updateVaccineType)
        {
            try
            {
                var vaccineType = await vaccineService.UpdateVaccineTypeAsync(id, updateVaccineType);
                return Ok($"vaccineType {vaccineType.Name} has been Updated");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [Authorize(Roles = "Doctor, Staff")]
        [HttpDelete("delete-vaccinetype/{id}")]
        public async Task<IActionResult> DeleteVaccineType([FromRoute] int id)
        {
            try
            {
                var vaccineType = await vaccineService.DeleteVaccineTypeAsync(id);
                return Ok($"vaccine {vaccineType.Name} has been deleted");
            }
            catch (Exception ex){ return BadRequest(ex.Message); }
        }

    //Vaccine 
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
        [HttpPut("update-vaccine/{id}")]
        public async Task<IActionResult> UpdateVaccine([FromRoute] int id, [FromBody] UpdateVaccineDto updateVaccineDto)
        {
            try
            {
                var vaccine = await vaccineService.UpdateVaccineAsync(id, updateVaccineDto);
                var vaccineDto = mapper.Map<VaccineDto>(vaccine);
                return Ok($"Updated vaccine {vaccineDto.VaccineID} successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Staff")]
        [HttpDelete("delete-vaccine")]
        public async Task<IActionResult> DeleteVaccine(int id)
        {
            try
            {
                var vaccine = await vaccineService.DeleteVaccineAsync(id);
                return Ok($"Vaccine {vaccine.Name} Deleted Successfully");
            }
            catch (Exception e) { return BadRequest(e.Message); }
        }
    }

}