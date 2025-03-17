using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Helpers;
using VaccineScheduleTracking.API_Test.Models.DTOs.Children;
using VaccineScheduleTracking.API_Test.Repository.Appointments;
using VaccineScheduleTracking.API_Test.Repository.Doctors;
using VaccineScheduleTracking.API_Test.Services.Accounts;
using VaccineScheduleTracking.API_Test.Services.Children;
using VaccineScheduleTracking.API_Test.Services.Doctors;
using static VaccineScheduleTracking.API_Test.Helpers.ExceptionHelper;


namespace VaccineScheduleTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChildController : ControllerBase
    {
        private readonly IChildService childService;
        private readonly IMapper mapper;
        private readonly IAccountService accountService;
        private readonly IDoctorRepository doctorRepository;
        private readonly IAppointmentRepository appointmentRepository;

        public ChildController(IAccountService accountService, IChildService childService, IMapper mapper, IDoctorRepository doctorRepository, IAppointmentRepository appointmentRepository)
        {
            this.childService = childService;
            this.mapper = mapper;
            this.accountService = accountService;
            this.doctorRepository = doctorRepository;
            this.appointmentRepository = appointmentRepository;
        }

        [Authorize(Policy = "EmailConfirmed")]
        [HttpGet]
        public async Task<IActionResult> GetChildren()
        {
            int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int currentUserID);
            var parentAccount = await accountService.GetAccountByIdAsync(currentUserID);
            if (parentAccount == null || parentAccount.Parent == null)
            {
                return BadRequest(new
                {
                    Message = "Không tìm thấy thông tin cha mẹ hợp lệ"
                });
            }
            return Ok(mapper.Map<List<ChildDto>>(await childService.GetParentChildren(parentAccount.Parent.ParentID)));
        }

        [Authorize(Roles = "Parent", Policy = "EmailConfirmed")]
        [HttpPost("add-child")]
        public async Task<IActionResult> CreateChildProfile([FromBody] AddChildDto addChild)
        {
            try
            {
                var child = mapper.Map<Child>(addChild);
                int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int currentUserID);
                var parentAccount = await accountService.GetAccountByIdAsync(currentUserID);
                if (parentAccount == null || parentAccount.Parent == null)
                {
                    return BadRequest(new
                    {
                        Message = "Không tìm thấy thông tin cha mẹ hợp lệ"
                    });
                }
                child.ParentID = parentAccount.Parent.ParentID;
                child = await childService.AddChild(child);
                return Ok(mapper.Map<ChildDto>(child));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Lấy danh sách trẻ của Parent đang đăng nhập
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Parent", Policy = "EmailConfirmed")]
        [HttpGet("get-childs-for-parent")]
        public async Task<IActionResult> GetChildListForParent()
        {
            try
            {
                var account = await accountService.GetAccountRole(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
                var childList = await childService.GetParentChildren(account.Parent.ParentID);

                return Ok(mapper.Map<List<ChildDto>>(childList));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [Authorize(Roles = "Parent", Policy = "EmailConfirmed")]
        [HttpPut("update-child/{id}")]
        public async Task<IActionResult> ModifileChildProfile([FromRoute] int id, [FromQuery] UpdateChildDto updateChild)
        {
            try
            {
                var parAccount = await accountService.GetAccountRole(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));


                var modifiledChild = await childService.UpdateChildForParent(parAccount.Parent.ParentID, id, mapper.Map<Child>(updateChild));

                return Ok(mapper.Map<ChildDto>(modifiledChild));
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Parent", Policy = "EmailConfirmed")]
        [HttpDelete("delete-child/{id}")]
        public async Task<IActionResult> DeleteChildProfile([FromRoute] int id)
        {
            try
            {
                var parAccount = await accountService.GetAccountRole(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));

                var deleteChild = await childService.DeleteChild(id, parAccount.Parent.ParentID);
                var appointments = await appointmentRepository.GetAppointmentsByChildIDAsync(id);

                if (appointments.IsNullOrEmpty())
                    foreach (var app in appointments)
                    {
                        await doctorRepository.DeleteDoctorTimeSlotByDoctorIDAsync(app.Account.Doctor.DoctorID);
                    }
                return Ok(new { Message = $"Child name {deleteChild.Firstname} has been deleted!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
