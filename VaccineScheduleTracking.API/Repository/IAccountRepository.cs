using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Repository
{
    public interface IAccountRepository 
    {
        Task<Account?> GetAccountByID(int id);
        Task<Account?> GetAccountByUsernameAsync(string username);
        Task<Account?> GetAccountByEmailAsync(string email);
        Task<Account?> GetAccountByPhonenumberAsync(string phonenumber);
        Task<Account> AddAsync(Account account);
        Task<Account?> UpdateAsync(UpdateAccountDto updateAccount);
    }
}
