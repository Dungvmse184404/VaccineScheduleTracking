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

        public async Task<Account> AddAsync(Account account)
        {
            await dbContext.Accounts.AddAsync(account);
            await dbContext.SaveChangesAsync();

            return account;
        }

        public async Task<Account?> GetAccountByID(int id)
        {
            return await dbContext.Accounts.FirstOrDefaultAsync(user => user.AccountID == id);
        }

        public async Task<Account?> UpdateAsync(UpdateAccountDto updateAccount)
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
    }
}
