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
using VaccineScheduleTracking.API_Test.Models.DTOs.Accounts;
using System.Security.Claims;
using VaccineScheduleTracking.API_Test.Repository.Doctors;
using VaccineScheduleTracking.API_Test.Services.Record;
using VaccineScheduleTracking.API_Test.Models.DTOs;
using System.Net.NetworkInformation;

namespace VaccineScheduleTracking.API_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorServices _doctorService;
        private readonly IDoctorRepository _doctorRepository;

        private readonly IAccountService _accountService;
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;

        public DoctorController(IDoctorServices doctorService,
                                IDoctorRepository doctorRepository,
                                IAccountService accountService,

                                IAppointmentService appointmentService,
                                IMapper mapper)
        {
            _doctorService = doctorService;
            _doctorRepository = doctorRepository;

            _accountService = accountService;
            _appointmentService = appointmentService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Staff", Policy = "EmailConfirmed")]
        [HttpGet("get-all-doctor")]
        public async Task<IActionResult> GetAllDoctor()
        {
            try
            {
                var docAccounts = await _doctorService.GetAllDoctorAsync();
                return Ok(_mapper.Map<List<DoctorAccountDto>>(docAccounts));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [Authorize(Roles = "Doctor", Policy = "EmailConfirmed")]
        [HttpGet("get-doctor-account")]
        public async Task<IActionResult> GetDoctorByAccountID()
        {
            try
            {
                var docAccount = await _accountService.GetAccountRole(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));

                ValidateInput(docAccount, $"bạn chưa đăng nhập");
                return Ok(_mapper.Map<DoctorAccountDto>(docAccount));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }

        }

        [Authorize(Roles = "Doctor", Policy = "EmailConfirmed")]
        [HttpPut("set-appointment-status/{appointmentId}")]
        public async Task<IActionResult> SetAppointmentStatus([FromRoute]int appointmentId, [FromQuery] string? note)
        {
            try
            {
                ValidateInput(appointmentId, "ID buổi hẹn không thể để trống");
                var appointment = await _appointmentService.SetAppointmentStatusAsync(appointmentId, "FINISHED", note);

                return Ok(_mapper.Map<AppointmentDto>(appointment));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }

        }


        [Authorize(Roles = "Doctor", Policy = "EmailConfirmed")]
        [HttpPut("change-doctor-schedule")]
        public async Task<IActionResult> ChangeDoctorTimeSlot([FromQuery] string doctorSchedule)
        {
            try
            {
                ValidateInput(doctorSchedule, "Chưa nhập lịch làm việc cho bác sĩ");
                ValidateDoctorSchedule(doctorSchedule);
                //------------------ hàm tạm để sửa lỗi ------------------
                var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var doc = await _doctorRepository.GetDoctorByAccountIDAsync(accountId);
                if (doc == null)
                {
                    throw new Exception($"không tìm thấy tài bác sĩ có acocuntId {accountId}");
                }
                int doctorId = doc.DoctorID;
                //--------------------------------------------------------

                //ValidateInput(await _doctorService.GetDoctorByIDAsync(doctorId), $"tài khoản {accountId} không có thẩm quyền của doctor");

                var appointmentList = await _appointmentService.GetPendingDoctorAppointmentAsync(doctorId);

                var appointments = await _doctorService.ReassignDoctorAppointmentsAsync(doctorId, appointmentList);
                foreach (var a in appointments)
                {
                    _appointmentService.UpdateAppointmentAsync(a.AppointmentID, _mapper.Map<UpdateAppointmentDto>(a));
                }
                var doctor = await _doctorService.ManageDoctorScheduleServiceAsync(doctorId, doctorSchedule);

                return Ok(_mapper.Map<List<AppointmentDto>>(appointments));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [Authorize(Roles = "Doctor", Policy = "EmailConfirmed")]
        [HttpDelete("delete-doctor-schedule")]
        public async Task<IActionResult> DeleteDoctor()
        {
            try
            {
                var account = await _doctorRepository.GetAccountByAccountIDAsync(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));

                await _doctorService.DeleteDoctorTimeSlotAsync(account.Doctor.DoctorID);
                return Ok($"đã xóa lịch làm việc ủa bác sĩ {account.Lastname} {account.Firstname}");
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

    }
}
