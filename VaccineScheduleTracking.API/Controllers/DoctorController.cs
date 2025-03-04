using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using static VaccineScheduleTracking.API_Test.Helpers.TimeSlotHelper;
using static VaccineScheduleTracking.API_Test.Helpers.ExceptionHelper;
using static VaccineScheduleTracking.API_Test.Helpers.ValidationHelper;
using Microsoft.AspNetCore.Mvc;
using VaccineScheduleTracking.API_Test.Models.DTOs.Doctors;
using VaccineScheduleTracking.API_Test.Services.Accounts;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Services.Doctors;
using VaccineScheduleTracking.API_Test.Repository.Appointments;
using VaccineScheduleTracking.API_Test.Services.Appointments;
using VaccineScheduleTracking.API_Test.Models.DTOs.Appointments;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Children;

namespace VaccineScheduleTracking.API_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {

        private readonly IDoctorServices _doctorService;

        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;

        public DoctorController(IDoctorServices doctorService, IAppointmentService appointmentService, IMapper mapper)
        {
            _doctorService = doctorService;

            _appointmentService = appointmentService;
            _mapper = mapper;
        }

        [HttpGet("get-doctors")]
        public async Task<IActionResult> GetAllDoctor()
        {
            try
            {
                var doctors = await _doctorService.GetAllDoctorAsync();
                return Ok(_mapper.Map<List<DoctorDto>>(doctors));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }


        }


        [HttpGet("get-doctors/{doctorId}")]
        public async Task<IActionResult> GetDoctorByID(int doctorId)
        {
            try
            {
                var doctor = await _doctorService.GetDoctorByIDAsync(doctorId);
                ValidateInput(doctor, $"Bác sĩ với ID {doctorId} không tồn tại");
                return Ok(_mapper.Map<DoctorDto>(doctor));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }


        }


        [HttpPut("set-appointment-status")]
        public async Task<IActionResult> SetAppointmentStatus(int appointmentId, string status)
        {
            try
            {
                var appointment = await _appointmentService.SetAppointmentStatusAsync(appointmentId, status);
                return Ok(_mapper.Map<AppointmentDto>(appointment));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }


        }



        [HttpPut("change-doctor-slot")]
        public async Task<IActionResult> ChangeDoctorTimeSlot(int doctorId, string doctorSchedule)
        {
            try
            {
                ValidateInput(_doctorService.GetDoctorByIDAsync(doctorId), $"không tìm thấy bác sĩ có ID {doctorId}");
                ValidateDoctorSchedule(doctorSchedule);

                var appointmentList = await _appointmentService.GetPendingDoctorAppointmentAsync(doctorId);


                var childSlotList = await _doctorService.ReassignDoctorAppointmentsAsync(doctorId, appointmentList);
                var doctor = await _doctorService.ManageDoctorScheduleServiceAsync(doctorId, doctorSchedule);

                return Ok(_mapper.Map<List<ChildTimeSlotDto>>(childSlotList));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("delete-doctor-schedule/{Id}")]
        public async Task<IActionResult> DeleteDoctor([FromRoute] int Id)
        {
            try
            {
                 await _doctorService.DeleteDoctorTimeSlotAsync(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

    }
}
