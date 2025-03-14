﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VaccineScheduleTracking.API_Test.Models.DTOs.Appointments;
using static System.Reflection.Metadata.BlobBuilder;
using static VaccineScheduleTracking.API_Test.Helpers.ValidationHelper;
using static VaccineScheduleTracking.API_Test.Helpers.ExceptionHelper;
using VaccineScheduleTracking.API_Test.Services.Appointments;
using VaccineScheduleTracking.API_Test.Repository.DailyTimeSlots;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using VaccineScheduleTracking.API_Test.Services.Accounts;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Services.Children;
using VaccineScheduleTracking.API_Test.Models.DTOs.Children;

namespace VaccineScheduleTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentServices;
        //private readonly ITimeSlotRepository _timeSlotRepository;

        private readonly IChildService _childService;
        private readonly IAccountService _accountServices;
        private readonly IMapper _mapper;

        public AppointmentController(IAppointmentService appointmentServices,
                                     IAccountService accountService,

                                     IChildService childService,
                                     IMapper mapper)
        {
            _appointmentServices = appointmentServices;
            _childService = childService;

            _accountServices = accountService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Parent")]
        [HttpPost("create-appointment")]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentDto createAppointment)
        {
            try
            {
                var appointment = await _appointmentServices.CreateAppointmentAsync(createAppointment);
                return Ok(_mapper.Map<AppointmentDto>(appointment));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [Authorize(Roles = "Doctor, Parent")]
        [HttpGet("get-appointment-by-appointmentId/{appointmentId}")]
        public async Task<IActionResult> GetAppointmentByAppointmentID([FromRoute] int appointmentId)
        {
            try
            {
                var appointment = await _appointmentServices.GetAppointmentByIDAsync(appointmentId);
                if (appointment == null)
                {
                    throw new Exception($"không tìm thấy Appointment có ID {appointmentId}");
                }
                return Ok(_mapper.Map<AppointmentDto>(appointment));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// lấy danh sách appointment trẻ cho Parent đang đăng nhập
        /// </summary>
        /// <param name="childId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Parent")]
        [HttpGet("get-parent-appointment-list/{childId}")]
        public async Task<IActionResult> GetParentAppointments([FromRoute] int childId)
        {
            try
            {
                ValidateInput(childId, "chưa nhập Id cho trẻ");
                var account = await _accountServices.GetAccountRole(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));

                var appointmens = await _appointmentServices.GetChildAppointmentsAsync(childId);
                return Ok(_mapper.Map<List<AppointmentDto>>(appointmens));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        /// <summary>
        /// lấy danh sách appointment trẻ cho Parent đang đăng nhập
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Doctor")]
        [HttpGet("get-doctor-appointment-list")]
        public async Task<IActionResult> GetDoctorAppointments()
        {
            try
            {
                var account = await _accountServices.GetAccountRole(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));

                var appointments = await _appointmentServices.GetDoctorAppointmentsAsync(account.Doctor.DoctorID);
                return Ok(_mapper.Map<List<AppointmentDto>>(appointments));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [HttpPut("update-appointment/{appointmentId}")]
        public async Task<IActionResult> UpdateAppointment([FromRoute] int appointmentId, UpdateAppointmentDto updateAppointment)
        {
            try
            {
                var appointment = await _appointmentServices.UpdateAppointmentAsync(appointmentId, updateAppointment);
                return Ok(_mapper.Map<AppointmentDto>(appointment));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [HttpDelete("cancel-appointment/{id}")]
        public async Task<IActionResult> CancelAppointment([FromRoute] int id)
        {
            try
            {
                var appointment = await _appointmentServices.CancelAppointmentAsync(id);
                return Ok(_mapper.Map<AppointmentDto>(appointment));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }



    }
}
