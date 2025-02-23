using System.Runtime.CompilerServices;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Accounts;


namespace VaccineScheduleTracking.API_Test.Services
{
    public interface IAccountService
    {
        Task<Account?> LoginAsync(string username, string password);
        Task<Account?> RegisterAsync(RegisterAccountDto registerAccount);
        Task<Account?> UpdateAccountAsync(UpdateAccountDto updateAccount);
        Task<List<Account>> GetAllAccountsAsync(FilterAccountDto filterAccountDto);
        Task<Account?> DisableAccountAsync(int id);
        Task<bool> VerifyAccountEmail(int accountId, string username, string email, string phoneNumber);
        //Task<Account?> DeleteAccountAsync(int id);
    }
}
