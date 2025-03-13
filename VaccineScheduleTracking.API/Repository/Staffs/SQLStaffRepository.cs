using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository.Staffs
{
    public class SQLStaffRepository : IStaffRepository
    {
        private readonly VaccineScheduleDbContext _dbContext;

        public SQLStaffRepository(VaccineScheduleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Account> AddStaffToAccountAsync(Account account, Staff staffInfo)
        {
            account.Staff = staffInfo;
            _dbContext.Accounts.Update(account);
            await _dbContext.SaveChangesAsync();
            return account;
        }
    }
}
