using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VaccineScheduleTracking.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Azure.Core;
using VaccineScheduleTracking.API_Test.Models.DTOs.Accounts;
using VaccineScheduleTracking.API_Test.Services.Accounts;
using VaccineScheduleTracking.API_Test.Repository.Accounts;

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

        public AccountController(IAccountRepository accountRepository, IMapper mapper, IAccountService accountService, JwtHelper jwtHelper)
        {
            this.accountRepository = accountRepository;
            this.mapper = mapper;
            this.accountService = accountService;
            this.jwtHelper = jwtHelper;
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
                return BadRequest(new { Message = ex.Message});
            }
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
