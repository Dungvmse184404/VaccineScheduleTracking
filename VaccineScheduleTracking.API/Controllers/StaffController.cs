using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Accounts;
using VaccineScheduleTracking.API_Test.Repository.Accounts;
using VaccineScheduleTracking.API_Test.Services.Accounts;
using VaccineScheduleTracking.API_Test.Services.Staffs;
using static VaccineScheduleTracking.API_Test.Helpers.ExceptionHelper;
using static VaccineScheduleTracking.API_Test.Helpers.ValidationHelper;

namespace VaccineScheduleTracking.API_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IStaffService _staffService;
        private readonly IMapper _mapper;

        public StaffController(IAccountService accountService,
                               IStaffService staffService,
                               IMapper mapper)
        {
            _accountService = accountService;
            _staffService = staffService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Staff", Policy = "EmailConfirmed")]
        [HttpPost("Promote-to-doctor/{accountId}")]
        public async Task<IActionResult> PromoteToDoctor([FromRoute] int accountId, [FromQuery] string? schedule)
        {
            try
            {
                ValidateInput(accountId, "chưa nhập ID cho account gán role doctor");
                ValidateDoctorSchedule(schedule);
                var account = await _staffService.PromoteToDoctorAsync(accountId, schedule);

                return Ok(_mapper.Map<DoctorAccountDto>(account));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [Authorize(Roles = "Staff", Policy = "EmailConfirmed")]
        [HttpPost("Promote-to-staff/{accountId}")]
        public async Task<IActionResult> PromoteToStaff([FromRoute] int accountId)
        {
            try
            {
                ValidateInput(accountId, "chưa nhập ID cho account gán role staff");
                var account = await _staffService.PromoteToStaffAsync(accountId);

                return Ok(_mapper.Map<StaffAccountDto>(account));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [Authorize(Roles = "Staff")]
        [HttpPost("Promote-to-manager/{accountId}")]
        public async Task<IActionResult> PromoteToManager([FromRoute] int accountId)
        {
            try
            {
                ValidateInput(accountId, "chưa nhập ID cho account gán role manager");
                var account = await _staffService.PromoteToManagerAsync(accountId);

                return Ok(_mapper.Map<ManagerAccountDto>(account));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


    }
}
