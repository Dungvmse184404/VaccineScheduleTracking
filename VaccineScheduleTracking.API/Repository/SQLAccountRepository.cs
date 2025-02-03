using Microsoft.EntityFrameworkCore;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Repository
{
    public class SQLAccountRepository : IAccountRepository
    {
        private readonly VaccineScheduleDbContext dbContext;

        public SQLAccountRepository(VaccineScheduleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<Account?> GetAccountByKeywordAsync(string keyword)
        {
            return await GetAccountByIDAsync(keyword)
                ?? await GetAccountByEmailAsync(keyword)
                ?? await GetAccountByUsernameAsync(keyword)
                ?? await GetAccountByPhonenumberAsync(keyword);
        }

        private async Task<Account?> GetAccountByIDAsync(string keyword)
        {
            if (int.TryParse(keyword, out int id))
            {
                return await GetAccountByID(id);
            }
            return null;
        }



        public async Task<Account?> GetAccountByID(int id)
        {
            return await dbContext.Accounts.FirstOrDefaultAsync(user => user.AccountID == id);
        }

        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            return await dbContext.Accounts.FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<Account?> GetAccountByPhonenumberAsync(string phonenumber)
        {
            return await dbContext.Accounts.FirstOrDefaultAsync(user => user.PhoneNumber == phonenumber);
        }

        public async Task<Account?> GetAccountByUsernameAsync(string username)
        {
            return await dbContext.Accounts.Include(x => x.Parent).
                                            Include(x => x.Doctor).
                                            Include(x => x.Staff).FirstOrDefaultAsync(user => user.Username == username);
        }

        public async Task<Account> AddAccountAsync(Account account)
        {
            await dbContext.Accounts.AddAsync(account);
            await dbContext.SaveChangesAsync();

            return account;
        }

        public async Task<Account?> UpdateAccountAsync(UpdateAccountDto updateAccount)
        {
            var account = await GetAccountByID(updateAccount.AccountID);
            if (account == null)
            {
                return null;
            }
            account.Firstname = updateAccount.Firstname;
            account.Lastname = updateAccount.Lastname;
            account.Email = updateAccount.Email;
            account.PhoneNumber = updateAccount.PhoneNumber;
            account.Avatar = updateAccount.Avatar;
            account.Password = updateAccount.Password;

            await dbContext.SaveChangesAsync();

            return account;
        }

        public async Task<List<Account>> GetAllAccountsAsync(FilterAccountDto filterAccountDto)
        {
            var query = dbContext.Accounts.Include(x => x.Parent).
                                           Include(x => x.Doctor).
                                           Include(x => x.Staff).AsQueryable();
            if (filterAccountDto.AccountID.HasValue)
            {
                query = query.Where(x => x.AccountID == filterAccountDto.AccountID);
            }
            if (!string.IsNullOrEmpty(filterAccountDto.Username))
            {
                query = query.Where(x => x.Username == filterAccountDto.Username);
            }
            if (!string.IsNullOrEmpty(filterAccountDto.Firstname))
            {
                query = query.Where(x => x.Firstname == filterAccountDto.Firstname);
            }
            if (!string.IsNullOrEmpty(filterAccountDto.Lastname))
            {
                query = query.Where(x => x.Lastname == filterAccountDto.Lastname);
            }
            if (!string.IsNullOrEmpty(filterAccountDto.Email))
            {
                query = query.Where(x => x.Email == filterAccountDto.Email);
            }
            if (!string.IsNullOrEmpty(filterAccountDto.PhoneNumber))
            {
                query = query.Where(x => x.PhoneNumber == filterAccountDto.PhoneNumber);
            }
            if (!string.IsNullOrEmpty(filterAccountDto.Status))
            {
                query = query.Where(x => x.Status == filterAccountDto.Status);
            }

            return await query.ToListAsync();
        }

        public async Task<Account?> DeleteAccountsAsync(Account account)
        {
            account.Status = "INACTIVE";
            await dbContext.SaveChangesAsync();

            return account;
        }
    }
}
