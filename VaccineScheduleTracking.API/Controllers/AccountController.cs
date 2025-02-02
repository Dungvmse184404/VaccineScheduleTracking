using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VaccineScheduleTracking.API.Helpers;
using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Repository;
using VaccineScheduleTracking.API.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Azure.Core;

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
                var token = jwtHelper.GenerateToken(account.AccountID.ToString() ,account.Username, role);
                return Ok(new
                {
                    Token = token,
                    AccountType = role,
                    Profile = mapper.Map<AccountDto>(account)
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
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
                return BadRequest(ex.Message);
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
                return BadRequest(ex.Message);  
            }
        }

    }
}
