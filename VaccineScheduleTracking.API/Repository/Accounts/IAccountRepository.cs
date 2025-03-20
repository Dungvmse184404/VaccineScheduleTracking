using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Accounts;
using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository.Accounts
{
    public interface IAccountRepository
    {
        Task<Account?> GetAccountRole(int accountId);
        Task<Account?> GetAccountByKeywordAsync(string keyword);
        Task<Account?> GetAccountByID(int id);
        Task<Account?> GetAccountByUsernameAsync(string username);
        Task<Account?> GetAccountByEmailAsync(string email);
        Task<Account?> GetAccountByPhonenumberAsync(string phonenumber);
        Task<Account> AddAccountAsync(Account account);
        Task<Account?> UpdateAccountAsync(Account updateAccount);
        Task<List<Account>> GetAllAccountsAsync(FilterAccountDto filterAccountDto);
        Task<Account?> DisableAccountAsync(Account account);
        Task<Account?> GetParentByChildIDAsync(int childID);
        Task CreateAccountNotationAsync(AccountNotation acc);
        Task<List<AccountNotation>> GetAllAccountNotationsAsync();
        Task<AccountNotation> GetAllAccountNotationByIDAsync(int accountID);
        Task UpdateAccountNoteAsync(AccountNotation accNote);
        Task<List<Account>> GetAllBlankAccountsAsync();
        Task<Account?> EnableAccountAsync(Account account);
        //Task<Account?> DeleteAccountsAsync(Account account);
    }
}
