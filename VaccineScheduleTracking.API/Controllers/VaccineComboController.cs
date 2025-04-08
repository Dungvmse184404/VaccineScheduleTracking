using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs;
using VaccineScheduleTracking.API_Test.Payments.VnPay.Models;
using VaccineScheduleTracking.API_Test.Payments.VnPay.Service;
using VaccineScheduleTracking.API_Test.Services.Accounts;
using VaccineScheduleTracking.API_Test.Services.VaccinePackage;
using static VaccineScheduleTracking.API_Test.Services.Appointments.AppointmentService;

namespace VaccineScheduleTracking.API_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineComboController : ControllerBase
    {
        private readonly IAccountService accountService;
        private readonly IVnPayService vnPayService;
        private readonly IVaccineComboService vaccineComboService;
        private readonly IMapper mapper;
        private readonly IMemoryCache cache;

        public VaccineComboController(IAccountService accountService, IVnPayService vnPayService, IVaccineComboService vaccineComboService, IMapper mapper, IMemoryCache cache)
        {
            this.accountService = accountService;
            this.vnPayService = vnPayService;
            this.vaccineComboService = vaccineComboService;
            this.mapper = mapper;
            this.cache = cache;
        }

        [HttpGet("get-all-vaccine-combo")]
        public async Task<IActionResult> GetVaccineCombos()
        {
            var vaccineCombos = await vaccineComboService.GetVaccineCombosAsync();

            return Ok(mapper.Map<List<VaccineComboDto>>(vaccineCombos));
        }

        [Authorize(Roles = "Manager, Doctor", Policy = "EmailConfirmed")]
        [HttpPost("create-vaccine-combo")]
        public async Task<IActionResult> CreateVaccineCombo([FromBody] CreateVaccineComboDto createVaccineCombo)
        {
            var vaccineCombo = await vaccineComboService.CreateVaccineComboAsync(createVaccineCombo);
            return Ok(mapper.Map<VaccineComboDto>(vaccineCombo));
        }

        [Authorize(Roles = "Doctor, Manager", Policy = "EmailConfirmed")]
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

        [Authorize(Roles = "Doctor, Manager", Policy = "EmailConfirmed")]
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

        [Authorize(Roles = "Doctor, Manager", Policy = "EmailConfirmed")]
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

        [Authorize(Roles = "Doctor, Manager", Policy = "EmailConfirmed")]
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

        //[Authorize(Roles = "Parent", Policy = "EmailConfirmed")]
        [HttpPost("register-combo")]
        public async Task<IActionResult> RegisterCombo([FromBody] RegisterComboDto regCombo)
        {
            try
            {
                var appointmentDtoList = await vaccineComboService.GenerateAppointmentsFromCombo(regCombo.StartDate, regCombo.ChildId, regCombo.ComnboId);
                
                var parent = await accountService.GetParentByChildIDAsync(regCombo.ChildId);
                if (parent == null)
                {
                    return BadRequest(new
                    {
                        Message = "Không tìm thấy thông tin tài khoản"
                    });
                }

                var cacheKey = $"combo_appointments_{regCombo.ComnboId}_{parent.AccountID}";
                cache.Set(cacheKey, appointmentDtoList, TimeSpan.FromMinutes(30));

                var PaymentModel = new PaymentInformationModel
                {
                    Amount = regCombo.Amount,
                    OrderDescription = regCombo.OrderDescription,
                    PaymentType = "combo",
                    AccountID = parent.AccountID,
                    AppointmentID = regCombo.ComnboId,

                };
                var url = vnPayService.CreatePaymentUrl(PaymentModel, HttpContext);
                
                return Ok(new { PaymentUrl = url });
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
