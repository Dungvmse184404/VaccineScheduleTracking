using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VaccineScheduleTracking.API.Services;
using VaccineScheduleTracking.API_Test.Models.DTOs;
using VaccineScheduleTracking.API_Test.Models.DTOs.Accounts;
using VaccineScheduleTracking.API_Test.Models.DTOs.Doctors;
using VaccineScheduleTracking.API_Test.Services.Accounts;

namespace VaccineScheduleTracking.API_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorServices _doctorService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        
        //[HttpGet("get-doctors")]
        //public async Task<IActionResult> GetAllDoctor([FromQuery]  )
        //{
        //    var doctors = await _doctorService.GetAllDoctorAsync(filterAccount);

        //    return Ok(_mapper.Map<List<DoctorDto>>(doctors));


        //}


    }
}
