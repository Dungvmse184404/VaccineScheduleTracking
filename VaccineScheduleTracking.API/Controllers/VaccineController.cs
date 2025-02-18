using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API.Helpers;
using VaccineScheduleTracking.API_Test.Models.DTOs.Vaccines;
using VaccineScheduleTracking.API_Test.Services.Vaccines;

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
        [HttpGet("getall-vaccinetype")]
        public async Task<IActionResult> GetAllVaccineType()
        {
            try
            {
                var vaccineType = await vaccineService.GetAllVaccineTypeAsync();
                return Ok(mapper.Map<List<FilterVaccineTypeDto>>(vaccineType));
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }


        }


        [Authorize(Roles = "Doctor, Staff")]
        [HttpPost("add-vaccinetype")]
        public async Task<IActionResult> CreateVaccineType([FromBody] AddVaccineTypeDto addVaccineTypeDto)
        {

            var vaccineType = await vaccineService.CreateVaccineTypeAsync(addVaccineTypeDto);
            if (vaccineType == null)
            {
                return BadRequest($"Vaccine {addVaccineTypeDto.Name} đã tồn tại");
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
                return Ok($"Đã cập nhật loại Vaccine {vaccineType.Name}");
            }
            catch (Exception ex) { return BadRequest( new { Message = ex.Message } ); }
        }


        [Authorize(Roles = "Doctor, Staff")]
        [HttpDelete("delete-vaccinetype/{id}")]
        public async Task<IActionResult> DeleteVaccineType([FromRoute] int id)
        {
            try
            {
                var vaccineType = await vaccineService.DeleteVaccineTypeAsync(id);
                return Ok($"Đã xóa loại Vaccine {vaccineType.Name} ");
            }
            catch (Exception ex) { return BadRequest( new { Message = ex.Message } ); }
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
                return BadRequest( new { Message = ex.Message } );
            }
        }


        [Authorize(Roles = "Doctor, Staff")]
        [HttpPut("update-vaccine/{id}")]
        public async Task<IActionResult> UpdateVaccine([FromRoute] int id, [FromBody] UpdateVaccineDto updateVaccineDto)
        {
            try
            {
                var vaccine = await vaccineService.UpdateVaccineAsync(id, updateVaccineDto);
                var vaccineDto = mapper.Map<VaccineDto>(vaccine);
                return Ok($"Cập nhật thành công Vaccine {vaccineDto.VaccineID}");
            }
            catch (Exception ex)
            {
                return BadRequest( new { Message = ex.Message } );
            }
        }

        [Authorize(Roles = "Doctor, Staff")]
        [HttpDelete("delete-vaccine")]
        public async Task<IActionResult> DeleteVaccine(int id)
        {
            try
            {
                var vaccine = await vaccineService.DeleteVaccineAsync(id);
                return Ok($"Đã xóa Vaccine {vaccine.Name}");
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }
    }

}