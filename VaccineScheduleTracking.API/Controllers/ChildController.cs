using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Services;

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
    }
}
