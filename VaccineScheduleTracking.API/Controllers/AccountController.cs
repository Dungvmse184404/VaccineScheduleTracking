using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VaccineScheduleTracking.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Azure.Core;
using VaccineScheduleTracking.API_Test.Models.DTOs.Accounts;
using VaccineScheduleTracking.API_Test.Services;
using VaccineScheduleTracking.API_Test.Repository;
using System.Security.Principal;
using VaccineScheduleTracking.API_Test.Models.DTOs;
using VaccineScheduleTracking.API_Test.Services.Accounts;
using VaccineScheduleTracking.API_Test.Repository.Accounts;
using VaccineScheduleTracking.API_Test.Configurations;
using VaccineScheduleTracking.API_Test.Services.Staffs;

namespace VaccineScheduleTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtHelper jwtHelper;
        private readonly IStaffService staffService;
        private readonly IAccountRepository accountRepository;
        private readonly IMapper mapper;
        private readonly IAccountService accountService;
        private readonly IEmailService emailService;

        public AccountController(JwtHelper jwtHelper,
                                IStaffService staffService,
                                IAccountRepository accountRepository,
                                IMapper mapper,
                                IAccountService accountService,
                                IEmailService emailService)
        {
            this.accountRepository = accountRepository;
            this.staffService = staffService;
            this.mapper = mapper;
            this.accountService = accountService;
            this.jwtHelper = jwtHelper;
            this.emailService = emailService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginAccountDto loginAccountDto)
        {
            try
            {
                var account = await accountService.LoginAsync(loginAccountDto.Username, loginAccountDto.Password);
                var role = account.Parent != null ? "Parent" : account.Doctor != null ? "Doctor" : account.Manager != null ? "Manager" : "Staff";
                var token = jwtHelper.GenerateToken(account.AccountID.ToString(), account.Username, role, account.Status);
                return Ok(new
                {
                    Token = token,
                    AccountType = role,
                    Profile = mapper.Map<AccountDto>(account)
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new
                {
                    Error = ex.StackTrace,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("register-parent")]
        public async Task<IActionResult> Register([FromForm] RegisterAccountDto registerAccount)
        {
            try
            {
                var account = await accountService.RegisterAsync(registerAccount);
                return Ok(mapper.Map<AccountDto>(account));
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Error = ex.StackTrace,
                    Message = ex.Message
                });
            }
        }


        [HttpPost("register-blank-account")]
        public async Task<IActionResult> RegisterBlankAccount([FromForm] RegisterBlankAccountDto registerAccount)
        {
            try
            {
                var account = await accountService.RegisterBlankAccountAsync(registerAccount);
                if (account != null)
                    await staffService.RecoveryAdminAccount(account, account.Password);
                if (account != null && registerAccount.Note != null)
                {
                    await accountService.CreateAccountNotation(account, registerAccount.Note);
                }

                return Ok(mapper.Map<AccountDto>(account));
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Error = ex.StackTrace,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("get-all-blank-accounts")]
        public async Task<IActionResult> GetAllBlankAccounts()
        {
            try
            {
                var accounts = await accountService.GetAllBlankAccountsAsync();

                return Ok(mapper.Map<AccountDto>(accounts));
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Error = ex.StackTrace,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("send-email-token")]
        public async Task<IActionResult> SendEmailToken(string username)
        {
            var account = await accountRepository.GetAccountByUsernameAsync(username);
            if (account == null)
            {
                return BadRequest(new
                {
                    Message = "Tài khoản không tồn tại"
                });
            }
            var token = jwtHelper.GenerateEmailToken(account.AccountID.ToString(), account.Username, account.Email, account.PhoneNumber);
            string verificationLink = $"https://localhost:7270/api/Account/verify-email?token={Uri.EscapeDataString(token)}";
            string emailBody =
            $@"<div style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px;'>
                <h2 style='color: #007bff;'>Xin chào {account.Firstname},</h2>
                <p>Chúng tôi rất vui khi bạn đăng ký tài khoản tại hệ thống của chúng tôi.</p>
                <p>Vui lòng nhấp vào nút bên dưới để xác minh email của bạn:</p>
                <div style='text-align: center; margin: 20px 0;'>
                    <a href='{verificationLink}' style='background-color: #007bff; color: #fff; padding: 12px 24px; text-decoration: none; font-size: 16px; border-radius: 5px; display: inline-block;'>
                        Xác minh Email
                    </a>
                </div>
                <p>Nếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email này.</p>
                <p><strong>Lưu ý:</strong> Liên kết sẽ hết hạn trong <strong>24 giờ</strong>.</p>
                <hr style='border: none; border-top: 1px solid #ddd; margin: 20px 0;'>
                <p style='text-align: center; font-size: 14px; color: #777;'>Trân trọng,<br>Đội ngũ hỗ trợ</p>
            </div>";
            await emailService.SendEmailAsync(account.Email, "Xác minh email", emailBody);

            return Ok(new
            {
                Message = "Link xác nhận đã được gửi vào email"
            });
        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Token không hợp lệ." });
            }
            var claims = jwtHelper.ValidateToken(token);
            if (claims == null || !claims.Any())
            {
                return BadRequest(new { message = "Token không hợp lệ hoặc đã hết hạn." });
            }

            var id = claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var email = claims.FirstOrDefault(c => c.Type == "Email")?.Value;
            var username = claims.FirstOrDefault(c => c.Type == "Username")?.Value;
            var phoneNumber = claims.FirstOrDefault(c => c.Type == "PhoneNumber")?.Value;

            var account = await accountRepository.GetAccountByEmailAsync(email);
            if (account == null || account.AccountID.ToString() != id || account.Username != username || account.PhoneNumber != phoneNumber)
            {
                return BadRequest(new
                {
                    ID = id,
                    Username = username,
                    PhoneNumber = phoneNumber,
                    Email = email,
                    Message = "Thông tin tài khoản không khớp."
                });
            }

            account.Status = "ACTIVE";
            await accountRepository.UpdateAccountAsync(account);

            return Ok(
                new
                {
                    Message = "Email của bạn đã được xác minh thành công!"
                });
        }

        [Authorize(Policy = "EmailConfirmed")]
        [HttpPut("update-account")]
        public async Task<IActionResult> UpdateAccount([FromForm] UpdateAccountDto updateAccount)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                if (currentUserId != updateAccount.AccountID)
                {
                    return BadRequest("You can't modifile account of another person.");
                }

                var account = await accountService.UpdateAccountAsync(updateAccount);

                return Ok(mapper.Map<AccountDto>(account));
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Doctor, Staff")]
        [HttpGet("get-accounts")]
        public async Task<IActionResult> GetAllAccounts([FromQuery] FilterAccountDto filterAccount)
        {
            var accounts = await accountService.GetAllAccountsAsync(filterAccount);

            return Ok(mapper.Map<List<AccountDto>>(accounts));
        }


        [Authorize(Roles = "Staff", Policy = "EmailConfirmed")]
        [HttpDelete("disable-account/{id}")]
        public async Task<IActionResult> DisableAccount([FromRoute] int id)
        {
            try
            {
                var account = await accountService.DisableAccountAsync(id);
                if (account == null)
                {
                    return NotFound("Account not found or already inactive");
                }
                //return Ok(mapper.Map<DeleteAccountDto>(account));
                return Ok($" Account {account.Username} disabled successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }

        //[Authorize (Roles = "Staff")]
        //[HttpDelete("delete-account")]
        //public async Task<IActionResult> deleteAccount([FromRoute] int id)
        //{
        //    try
        //    {
        //        var account = await accountService.DeleteAccountAsync(id);
        //        if (account == null)
        //        {
        //            return NotFound("Account not found or already inactive");
        //        }
        //        //return Ok(mapper.Map<DeleteAccountDto>(account));
        //        return Ok($" Account {account.Username} deleted successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //}

    }
}
