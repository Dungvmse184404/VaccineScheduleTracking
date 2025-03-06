using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Children;
using VaccineScheduleTracking.API_Test.Services.Accounts;
using VaccineScheduleTracking.API_Test.Services.Children;

namespace VaccineScheduleTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChildController : ControllerBase
    {
        private readonly IChildService childService;
        private readonly IMapper mapper;
        private readonly IAccountService accountService;

        public ChildController(IChildService childService, IMapper mapper, IAccountService accountService)
        {
            this.childService = childService;
            this.mapper = mapper;
            this.accountService = accountService;
        }

        [Authorize]
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

        [Authorize(Roles ="Parent")]
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

        [Authorize]
        [HttpPut("update-child/{id}")]
        public async Task<IActionResult> ModifileChildProfile(int id, [FromBody] UpdateChildDto updateChild)
        {
            try
            {
                var modifiledChild = await childService.UpdateChild(id, mapper.Map<Child>(updateChild));

                return Ok(mapper.Map<ChildDto>(modifiledChild));
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("delete-child/{id}")]
        public async Task<IActionResult> DeleteChildProfile(int id)
        {
            try
            {
                var deleteChild = await childService.DeleteChild(id);
                return Ok($"Child name {deleteChild.Firstname} has been deleted!");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
