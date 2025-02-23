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

namespace VaccineScheduleTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository accountRepository;
        private readonly IMapper mapper;
        private readonly IAccountService accountService;
        private readonly JwtHelper jwtHelper;
        private readonly IEmailService emailService;

        public AccountController(IAccountRepository accountRepository, IMapper mapper, IAccountService accountService, JwtHelper jwtHelper, IEmailService emailService)
        {
            this.accountRepository = accountRepository;
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
                var role = account.Parent != null ? "Parent" : account.Doctor != null ? "Doctor" : "Staff";
                var token = jwtHelper.GenerateToken(account.AccountID.ToString(), account.Username, role);
                return Ok(new
                {
                    Token = token,
                    AccountType = role,
                    Profile = mapper.Map<AccountDto>(account)
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }

        [HttpPost("register-parent")]
        public async Task<IActionResult> Register([FromBody] RegisterAccountDto registerAccount)
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
            $@"<p>Xin chào {account.Firstname},</p>
                    <p>Vui lòng nhấp vào đường link dưới đây để xác minh email:</p>
                    <p><a href='{verificationLink}'>Xác minh Email</a></p>
                    <p>Liên kết sẽ hết hạn trong 24 giờ.</p>";
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

            // Cập nhật trạng thái xác minh email
            account.Status = "ACTIVE";
            await accountRepository.UpdateAccountAsync(mapper.Map<UpdateAccountDto>(account));

            return Ok(
                new
                {
                    Message = "Email của bạn đã được xác minh thành công!"
                });
        }

        [Authorize]
        [HttpPut("update-account")]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountDto updateAccount)
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


        [Authorize(Roles = "Staff")]
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
