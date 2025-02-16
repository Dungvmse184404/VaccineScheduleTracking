using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API.Repository;
using VaccineScheduleTracking.API.Services;
using VaccineScheduleTracking.API_Test.Repository;
using static System.Reflection.Metadata.BlobBuilder;

namespace VaccineScheduleTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentRepository;
        private readonly ITimeSlotRepository _timeSlotRepository;
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;

        public AppointmentController(IAppointmentService appointmentRepository, IAppointmentService appointmentService, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _appointmentService = appointmentService;
            _mapper = mapper;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetALlAppointment(AppointmentDto appointmentDto)
        //{
        //    var appointment = await _appointmentService.GetAllAppointmentAsync(appointmentDto);

        //    return Ok(_mapper.Map<AppointmentDto>(appointment));
        //}

        [HttpPost("create-appointment")]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentDto createAppointment)
        {
            try
            {

                var appointment = await _appointmentService.CreateAppointmentAsync(createAppointment);
                return Ok(_mapper.Map<AppointmentDto>(appointment));

            }
            catch (Exception e) { return BadRequest(e.Message); }
        }


        //[HttpGet("available-slots/{date}")]
        //public async Task<IActionResult> GetAvailableSlots(DateTime date)
        //{
        //    try
        //    {
        //        var slots = await _timeSlotRepository.GetAllSlotAsync(date);
        //        return Ok(_mapper.Map<List<DailyScheduleDto>>(slots));

        //    }
        //    catch (Exception e) { return BadRequest(e.Message); }

        //}


        [HttpGet("get-appointment-list")]
        public async Task<IActionResult> GetAppointmentByID(int id, string role)
        {
            try
            {
                var Appointment = await _appointmentRepository.GetAppointmentListByIDAsync(id, role);
                return Ok(_mapper.Map<List<AppointmentDto>>(Appointment));
            }
            catch (Exception e) { return BadRequest(e.Message); }
        }
    }
}
