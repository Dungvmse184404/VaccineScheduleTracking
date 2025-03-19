using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Accounts;
using VaccineScheduleTracking.API_Test.Models.DTOs.Doctors;
using VaccineScheduleTracking.API_Test.Models.DTOs.Mails;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API_Test.Repository.Accounts;
using VaccineScheduleTracking.API_Test.Services;
using VaccineScheduleTracking.API_Test.Services.Accounts;
using VaccineScheduleTracking.API_Test.Services.Staffs;
using static VaccineScheduleTracking.API_Test.Helpers.ExceptionHelper;
using static VaccineScheduleTracking.API_Test.Helpers.ValidationHelper;

namespace VaccineScheduleTracking.API_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IAccountService _accountService;
        private readonly IStaffService _staffService;
        private readonly IMapper _mapper;

        public StaffController(IEmailService emailService, IAccountService accountService, IStaffService staffService, IMapper mapper)
        {
            _emailService = emailService;
            _accountService = accountService;
            _staffService = staffService;
            _mapper = mapper;
        }

        private AutoMailDto CreateRoleAssignmentMailDto(string accountName, string role)
        {
            return new AutoMailDto
            {
                Footer = "Trân trọng,<br>Đội ngũ quản trị viên",
                RecipientName = accountName,
                Subject = "Thông báo cấp quyền truy cập",
                Body = $@"
                    Chúng tôi xin thông báo rằng tài khoản của bạn đã được cấp quyền mới trên hệ thống |Vaccine Schedule Tracking System|.<br>
                    |Vai trò mới:| {role} 📜<br>
                    📍 Vui lòng đăng nhập vào hệ thống để kiểm tra quyền hạn và sử dụng các tính năng tương ứng.<br>
                    Nếu bạn có bất kỳ thắc mắc nào, vui lòng liên hệ với quản trị viên để được hỗ trợ."
            };

        }

        


        [Authorize(Roles = "Staff", Policy = "EmailConfirmed")]
        [HttpGet("Get-All-Request")]
        public async Task<IActionResult> GetAllRequest()
        {
            try
            {
                var accNotes = await _accountService.GetAllAccountNotationsAsync();
                var blkAccounts = await _accountService.GetAllBlankAccountsAsync();

                var blankAccountDtos = blkAccounts
                    .Join(accNotes,
                          blkAcc => blkAcc.AccountID,
                          accNote => accNote.AccountID,
                          (blkAcc, accNote) => new BlankAccountDto
                          {
                              AccountID = blkAcc.AccountID,
                              Username = blkAcc.Username,
                              Firstname = blkAcc.Firstname,
                              Lastname = blkAcc.Lastname,
                              Email = blkAcc.Email,
                              PhoneNumber = blkAcc.PhoneNumber,
                              Status = blkAcc.Status,
                              Notation = accNote
                          })
                    .ToList();

                return Ok(blankAccountDtos);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [Authorize(Roles = "Staff", Policy = "EmailConfirmed")]
        [HttpPost("Promote-to-doctor")]
        public async Task<IActionResult> PromoteToDoctor([FromBody] PromoteDoctorDto doctorDto)
        {
            try
            {
                ValidateInput(doctorDto.AccountID, "chưa nhập ID cho account gán role doctor");
                ValidateDoctorSchedule(doctorDto.DoctorSchedule);
                var account = await _staffService.PromoteToDoctorAsync(doctorDto.AccountID, doctorDto.DoctorSchedule);
                if (account == null)
                {
                    var mail = CreateRoleAssignmentMailDto($"{account.Lastname} {account.Firstname}", "Bác sĩ");
                    await _emailService.SendEmailAsync(account.Email, mail.Subject, mail.Body);
                    await _accountService.SetAccountNotationsAsync(account.AccountID, true);
                }

                return Ok(_mapper.Map<DoctorAccountDto>(account));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        //[Authorize(Roles = "Staff", Policy = "EmailConfirmed")]
        [HttpPost("Promote-to-staff/{accountId}")]
        public async Task<IActionResult> PromoteToStaff([FromRoute] int accountId)
        {
            try
            {
                ValidateInput(accountId, "chưa nhập ID cho account gán role staff");
                var account = await _staffService.PromoteToStaffAsync(accountId);
                if (account != null)
                {
                    var mail = CreateRoleAssignmentMailDto($"{account.Lastname} {account.Firstname}", "Quản trị viên");
                    await _emailService.SendEmailAsync(account.Email, mail.Subject, mail.Body);
                    await _accountService.SetAccountNotationsAsync(account.AccountID, true);
                }

                return Ok(_mapper.Map<StaffAccountDto>(account));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [Authorize(Roles = "Staff")]
        [HttpPost("Promote-to-manager/{accountId}")]
        public async Task<IActionResult> PromoteToManager([FromRoute] int accountId)
        {
            try
            {
                ValidateInput(accountId, "chưa nhập ID cho account gán role manager");
                var account = await _staffService.PromoteToManagerAsync(accountId);
                if (account != null)
                {
                    var mail = CreateRoleAssignmentMailDto($"{account.Lastname} {account.Firstname}", "Quản lý viên");
                    await _emailService.SendEmailAsync(account.Email, mail.Subject, mail.Body);
                }
                return Ok(_mapper.Map<ManagerAccountDto>(account));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


    }
}
