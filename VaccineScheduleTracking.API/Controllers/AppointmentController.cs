using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VaccineScheduleTracking.API_Test.Models.DTOs.Appointments;
using static System.Reflection.Metadata.BlobBuilder;
using static VaccineScheduleTracking.API_Test.Helpers.ExceptionHelper;
using VaccineScheduleTracking.API_Test.Services.Appointments;
using VaccineScheduleTracking.API_Test.Repository.DailyTimeSlots;

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


        [HttpPost("create-appointment")]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentDto createAppointment)
        {
            try
            {
                var appointment = await _appointmentService.CreateAppointmentAsync(createAppointment);
                return Ok(_mapper.Map<AppointmentDto>(appointment));

            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [HttpGet("get-appointment-list")]
        public async Task<IActionResult> GetAppointmentByID(int id, string role)
        {
            try
            {
                var Appointment = await _appointmentRepository.GetAppointmentListByIDAsync(id, role);
                return Ok(_mapper.Map<List<AppointmentDto>>(Appointment));
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpPut("update-appointment")]
        public async Task<IActionResult> UpdateAppointment([FromQuery] UpdateAppointmentDto updateAppointment)
        {
            try
            {
                var appointment = await _appointmentService.UpdateAppointmentAsync(updateAppointment);
                return Ok(_mapper.Map<AppointmentDto>(appointment));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
