﻿    using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using System.Numerics;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Accounts;
using VaccineScheduleTracking.API_Test.Models.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VaccineScheduleTracking.API_Test.Repository.Accounts
{
    public class SQLAccountRepository : IAccountRepository
    {
        private readonly VaccineScheduleDbContext dbContext;

        public SQLAccountRepository(VaccineScheduleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Account?> GetAccountRole(int accountId)
        {
            return await dbContext.Accounts
                               .Include(a => a.Parent)
                               .Include(a => a.Doctor)
                               .Include(a => a.Staff)
                               .Include(a => a.Manager)
                               .FirstOrDefaultAsync(a => a.AccountID == accountId);
        }


        public async Task<Account?> GetAccountByKeywordAsync(string keyword)
        {
            if (int.TryParse(keyword, out int id))
            {
                return await GetAccountByID(id);
            }
            return await GetAccountByEmailAsync(keyword)
                ?? await GetAccountByUsernameAsync(keyword)
                ?? await GetAccountByPhonenumberAsync(keyword);
        }

        public async Task<Account?> GetAccountByID(int id)
        {
            return await dbContext.Accounts.Include(x => x.Parent)
                                           .Include(x => x.Doctor)
                                           .Include(x => x.Staff)
                                           .Include(a => a.Manager)
                                           .FirstOrDefaultAsync(user => user.AccountID == id);
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
                                            Include(x => x.Staff).
                                            Include(a => a.Manager).
                                            FirstOrDefaultAsync(user => user.Username == username);
        }

        public async Task<Account> AddAccountAsync(Account account)
        {
            await dbContext.Accounts.AddAsync(account);
            await dbContext.SaveChangesAsync();

            return account;
        }

        public async Task<Account?> UpdateAccountAsync(Account updateAccount)
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
                                           Include(x => x.Staff).
                                           Include(a => a.Manager).
                                           AsQueryable();
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

        public async Task<Account?> DisableAccountAsync(Account account)
        {
            //account.Status = "INACTIVE";
            await dbContext.SaveChangesAsync();

            return account;
        }

        public async Task<Account?> EnableAccountAsync(Account account)
        {
            //account.Status = "ACTIVE";
            await dbContext.SaveChangesAsync();

            return account;
        }

        public async Task<Account?> GetParentByChildIDAsync(int childId)
        {
            return await dbContext.Children
                .Where(c => c.ChildID == childId)
                .Select(c => c.Parent.Account)
                .FirstOrDefaultAsync();
        }

        public async Task CreateAccountNotationAsync(AccountNotation acc)
        {
            await dbContext.AccountNotations.AddAsync(acc);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<AccountNotation>> GetAllAccountNotationsAsync()
        {
            return await dbContext.AccountNotations.ToListAsync();
        }

        public async Task<AccountNotation> GetAllAccountNotationByIDAsync(int accountID)
        {
            return await dbContext.AccountNotations.FirstOrDefaultAsync(x => x.AccountID == accountID);
        }

        public async Task UpdateAccountNoteAsync(AccountNotation accNote)
        {
            var oldAccNote = await GetAllAccountNotationByIDAsync(accNote.AccountID);
            if (oldAccNote == null)
            {
                throw new Exception("Account notation không tồn tại.");
            }
            oldAccNote.Notation = accNote.Notation;
            oldAccNote.Processed = accNote.Processed;
            oldAccNote.CreateDate = accNote.CreateDate;

            await dbContext.SaveChangesAsync();
        }

        public Task<List<Account>> GetAllBlankAccountsAsync()
        {
            return dbContext.Accounts.Where(x => x.Parent == null && x.Doctor == null && x.Staff == null && x.Manager == null).ToListAsync();
        }



        //public async Task<Account?> DeleteAccountsAsync(Account account)
        //{
        //    dbContext.Remove(account);
        //    await dbContext.SaveChangesAsync();

        //    return account;
        //}

    }
}
