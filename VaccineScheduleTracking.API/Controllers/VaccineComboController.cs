using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VaccineScheduleTracking.API_Test.Models.DTOs;
using VaccineScheduleTracking.API_Test.Services.VaccinePackage;
using static VaccineScheduleTracking.API_Test.Services.Appointments.AppointmentService;

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

        [HttpGet("get-all-vaccine-combo")]
        public async Task<IActionResult> GetVaccineCombos()
        {
            var vaccineCombos = await vaccineComboService.GetVaccineCombosAsync();

            return Ok(mapper.Map<List<VaccineComboDto>>(vaccineCombos));
        }

        [Authorize(Roles = "Doctor", Policy = "EmailConfirmed")]
        [HttpPost("create-vaccine-combo")]
        public async Task<IActionResult> CreateVaccineCombo([FromBody] CreateVaccineComboDto createVaccineCombo)
        {
            var vaccineCombo = await vaccineComboService.CreateVaccineComboAsync(createVaccineCombo);
            return Ok(mapper.Map<VaccineComboDto>(vaccineCombo));
        }

        [HttpGet("get-vaccine-combo/{comboID}")]
        public async Task<IActionResult> GetVaccineComboByID([FromRoute] int comboID)
        {
            var vaccineCombo = await vaccineComboService.GetVaccineComboByIdAsync(comboID);
            if (vaccineCombo == null)
            {
                return BadRequest(new
                {
                    Message = "Không tìm thấy Vaccine combo hợp lệ"
                });
            }
            return Ok(mapper.Map<VaccineComboDto>(vaccineCombo));
        }

        [HttpPost("add-vaccine-container")]
        public async Task<IActionResult> AddVaccineContainer(CreateVaccineContainerDto createVaccineContainer)
        {
            try
            {
                var vaccineContainer = await vaccineComboService.AddVaccineContainerAsync(createVaccineContainer);
                var vaccineCombo = await vaccineComboService.GetVaccineComboByIdAsync(createVaccineContainer.VaccineComboID);
                return Ok(mapper.Map<VaccineComboDto>(vaccineCombo));
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message,
                });
            }
        }

        [Authorize(Roles = "Doctor", Policy = "EmailConfirmed")]
        [HttpDelete("delete-vaccine-container")]
        public async Task<IActionResult> DeleteVaccineContainer([FromBody] DeleteVaccineContainerDto deleteVaccineContainer)
        {
            try
            {
                await vaccineComboService.DeleteVaccineContainerAsync(deleteVaccineContainer);
                return Ok(new
                {
                    Message = "Đã xóa vaccine container thành công"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message,
                });
            }
        }

        [Authorize(Roles = "Doctor", Policy = "EmailConfirmed")]
        [HttpDelete("delete-vaccine-combo/{vaccineComboID}")]
        public async Task<IActionResult> DeleteVaccineCombo([FromRoute] int vaccineComboID)
        {
            try
            {
                await vaccineComboService.DeleteVaccineComboAsync(vaccineComboID);
                return Ok(new
                {
                    Message = "Đã xóa vaccine combo thành công"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message,
                });
            }
        }

        [Authorize(Roles = "Parent", Policy = "EmailConfirmed")]
        [HttpPut("register-combo")]
        public async Task<IActionResult> RegisterCombo([FromBody] RegisterComboDto regCombo)
        {
            try
            {
               var error = await vaccineComboService.RegisterCombo(regCombo.StartDate, regCombo.ChildId, regCombo.ComnboId);
                if (error != null && error.Any())
                {
                    throw new Exception($"{error}, ");
                }
                return Ok("đăng kí combo thành công");
                
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message,
                });
            }
        }
    }
}
