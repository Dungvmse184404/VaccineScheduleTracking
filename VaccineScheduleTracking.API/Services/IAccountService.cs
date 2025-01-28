using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Services
{
    public interface IAccountService
    {
        Task<Account?> LoginAsync(string username, string password);
        Task<Account?> RegisterAsync(RegisterAccountDto registerAccount);
        Task<Account?> UpdateAsync(UpdateAccountDto updateAccount);
    }
}
