using AutoMapper;
using Microsoft.AspNetCore.Identity;
using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API.Repository;

namespace VaccineScheduleTracking.API.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository accountRepository;
        private readonly IMapper mapper;

        public AccountService(IAccountRepository accountRepository, IMapper mapper)
        {
            this.accountRepository = accountRepository;
            this.mapper = mapper;
        }
        public async Task<Account?> LoginAsync(string username, string password)
        {
            var account = await accountRepository.GetAccountByUsernameAsync(username);
            if (account == null)
            {
                throw new Exception("The account does not exist!");
            }
            if (account.Password != password)
            {
                throw new Exception("The password is invalid!");
            }
            return mapper.Map<Account>(account);
        }

        public async Task<Account?> RegisterAsync(RegisterAccountDto registerAccount)
        {
            if (await accountRepository.GetAccountByUsernameAsync(registerAccount.Username) != null)
            {
                throw new Exception($"{registerAccount.Username} is not available");
            }
            if (await accountRepository.GetAccountByEmailAsync(registerAccount.Email) != null)
            {
                throw new Exception($"{registerAccount.Email} is not available");
            }
            if (await accountRepository.GetAccountByPhonenumberAsync(registerAccount.PhoneNumber) != null)
            {
                throw new Exception($"{registerAccount.PhoneNumber} is not available");
            }

            var account = mapper.Map<Account>(registerAccount);
            account.Status = "ACTIVE";
            account.Parent = new Parent() { Account = account };

            return await accountRepository.AddAccountAsync(account);
        }

        public async Task<Account?> UpdateAccountAsync(UpdateAccountDto updateAccount)
        {
            var email = await accountRepository.GetAccountByEmailAsync(updateAccount.Email);
            if (email != null && email.AccountID != updateAccount.AccountID)
            {
                throw new Exception($"{updateAccount.Email} is not available");
            }
            var phoneNumber = await accountRepository.GetAccountByPhonenumberAsync(updateAccount.PhoneNumber);
            if (phoneNumber != null && phoneNumber.AccountID != updateAccount.AccountID)
            {
                throw new Exception($"{updateAccount.PhoneNumber} is not available");
            }
            var account = await accountRepository.UpdateAccountAsync(updateAccount);
            if (account == null)
            {
                throw new Exception("Account does not exist!");
            }
            return account;
        }

        public async Task<List<Account>> GetAllAccountsAsync(FilterAccountDto filterAccount)
        {
            return await accountRepository.GetAllAccountsAsync(filterAccount);
        }


        public async Task<Account?> DeleteAccountAsync(string keyword)
        {
            var account = await accountRepository.GetAccountByKeywordAsync(keyword);
            if (account == null)
            {
                throw new Exception($"{keyword} is not available");
            }
            account = mapper.Map<Account>(account);
            return await accountRepository.DeleteAccountsAsync(account);
        }
    }
}
