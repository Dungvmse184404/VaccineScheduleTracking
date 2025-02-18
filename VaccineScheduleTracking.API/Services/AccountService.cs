using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Transactions;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Accounts;
using VaccineScheduleTracking.API_Test.Repository;

namespace VaccineScheduleTracking.API_Test.Services
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
                throw new Exception("Tài khoản không tồn tại!");
            }
            if (account.Password != password)
            {
                throw new Exception("Sai mật khẩu!");
            }
            return mapper.Map<Account>(account);
        }

        public async Task<Account?> RegisterAsync(RegisterAccountDto registerAccount)
        {
            if (await accountRepository.GetAccountByUsernameAsync(registerAccount.Username) != null)
            {
                throw new Exception($"Tên người dùng: {registerAccount.Username} đã tồn tại");
            }
            if (await accountRepository.GetAccountByEmailAsync(registerAccount.Email) != null)
            {
                throw new Exception($"Địa chỉ email {registerAccount.Email} đã tồn tại ");
            }
            if (await accountRepository.GetAccountByPhonenumberAsync(registerAccount.PhoneNumber) != null)
            {
                throw new Exception($"Số điện thoại {registerAccount.PhoneNumber} đã tồn tại");
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
                throw new Exception($"Email {updateAccount.Email} đã tồn tại");
            }
            var phoneNumber = await accountRepository.GetAccountByPhonenumberAsync(updateAccount.PhoneNumber);
            if (phoneNumber != null && phoneNumber.AccountID != updateAccount.AccountID)
            {
                throw new Exception($"Số điện thoại {updateAccount.PhoneNumber} đã tồn tại");
            }
            var account = await accountRepository.UpdateAccountAsync(updateAccount);
            if (account == null)
            {
                throw new Exception($"Tài khoản không tồn tại!");
            }
            return account;
        }

        public async Task<List<Account>> GetAllAccountsAsync(FilterAccountDto filterAccount)
        {
            return await accountRepository.GetAllAccountsAsync(filterAccount);
        }


        public async Task<Account?> DisableAccountAsync(int id)
        {
            var account = await accountRepository.GetAccountByID(id);
            if (account == null)
            {
                throw new Exception($"ID {id} không tồn tại");
            }
            //account = mapper.Map<Account>(account);
            return await accountRepository.DisableAccountAsync(account);
        }




        //public async Task<Account?> DeleteAccountAsync(int id)
        //{
        //    var account = await accountRepository.GetAccountByID(id);
        //    if (account == null)
        //    {
        //        throw new Exception($"ID {id} is not available");
        //    }
        //    account = mapper.Map<Account>(account);
        //    return await accountRepository.DeleteAccountsAsync(account);
        //}
    }
}
