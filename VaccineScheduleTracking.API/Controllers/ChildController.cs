using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Children;
using VaccineScheduleTracking.API_Test.Services.Children;

namespace VaccineScheduleTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChildController : ControllerBase
    {
        private readonly IChildService childService;
        private readonly IMapper mapper;

        public ChildController(IChildService childService, IMapper mapper)
        {
            this.childService = childService;
            this.mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetChildren()
        {
            var currentUserID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return Ok(mapper.Map<List<ChildDto>>(await childService.GetParentChildren(currentUserID)));
        }

        [Authorize]
        [HttpPost("add-child")]
        public async Task<IActionResult> CreateChildProfile([FromBody] AddChildDto addChild)
        {
            var child = mapper.Map<Child>(addChild);
            var currentUserID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            child.ParentID = currentUserID;
            child = await childService.AddChild(child);
            return Ok(child);
        }

        [Authorize]
        [HttpPut("update-child{id}")]
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
        [HttpDelete("delete-child{id}")]
        public async Task<IActionResult> DeleteChildProfile(int ChildId)
        {
            try
            {
                var deleteChild = await childService.DeleteChild(ChildId);
                return Ok($"Child name {deleteChild.Firstname} has been deleted!");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
