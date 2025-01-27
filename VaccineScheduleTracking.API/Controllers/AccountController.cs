using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VaccineScheduleTracking.API.Helpers;
using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Repository;

namespace VaccineScheduleTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository accountRepository;
        private readonly IMapper mapper;
        private readonly JwtHelper jwtHelper;

        public AccountController(IAccountRepository accountRepository, IMapper mapper, JwtHelper jwtHelper)
        {
            this.accountRepository = accountRepository;
            this.mapper = mapper;
            this.jwtHelper = jwtHelper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginAccountDto loginAccountDto)
        {
            var account = await accountRepository.GetAccountByUsernameAsync(loginAccountDto.Username);

            if (account == null)
            {
                return Unauthorized(new { Message = "Account does not exist!" });
            }

            if (loginAccountDto.Password != account.Password)
            {
                return Unauthorized(new { Message = "Wrong account or password" });
            }

            var role = account.Parent != null ? "PARENT" : account.Doctor != null ? "DOCTOR" : "STAFF";

            var token = jwtHelper.GenerateToken(account.Username, account.Password);

            return Ok(new
            {
                Token = token,
                Profile = mapper.Map<AccountDto>(account)
            });
        }
    }
}
