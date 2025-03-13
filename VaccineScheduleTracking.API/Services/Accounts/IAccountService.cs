using System.Runtime.CompilerServices;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Accounts;
using VaccineScheduleTracking.API_Test.Models.Entities;


namespace VaccineScheduleTracking.API_Test.Services.Accounts
{
    public interface IAccountService
    {
        Task<Account?> LoginAsync(string username, string password);
        Task<Account?> RegisterAsync(RegisterAccountDto registerAccount);
        Task<Account?> RegisterBlankAccountAsync(RegisterAccountDto registerAccount);
        Task<Account?> UpdateAccountAsync(UpdateAccountDto updateAccount);
        Task<Account?> GetAccountRole(int accountId);
        Task<List<Account>> GetAllAccountsAsync(FilterAccountDto filterAccountDto);
        Task<Account?> DisableAccountAsync(int id);
        Task<bool> VerifyAccountEmail(int accountId, string username, string email, string phoneNumber);
        Task<Account?> GetAccountByIdAsync(int accountId);
        //Task<Account?> DeleteAccountAsync(int id);
    }
}
