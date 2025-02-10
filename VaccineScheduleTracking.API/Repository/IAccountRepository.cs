using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Repository
{
    public interface IAccountRepository 
    {
        Task<Account?> GetAccountByKeywordAsync(string keyword);
        Task<Account?> GetAccountByID(int id);
        Task<Account?> GetAccountByUsernameAsync(string username);
        Task<Account?> GetAccountByEmailAsync(string email);
        Task<Account?> GetAccountByPhonenumberAsync(string phonenumber);
        Task<Account> AddAccountAsync(Account account);
        Task<Account?> UpdateAccountAsync(UpdateAccountDto updateAccount);
        Task<List<Account>> GetAllAccountsAsync(FilterAccountDto filterAccountDto);
        Task<Account?> DisableAccountAsync(Account account);
        //Task<Account?> DeleteAccountsAsync(Account account);
    }
}
