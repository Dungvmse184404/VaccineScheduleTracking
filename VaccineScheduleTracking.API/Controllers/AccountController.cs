using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        public AccountController(IAccountRepository accountRepository, IMapper mapper)
        {
            this.accountRepository = accountRepository;
            this.mapper = mapper;
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

            return Ok(mapper.Map<AccountDto>(account));
        }
    }
}
