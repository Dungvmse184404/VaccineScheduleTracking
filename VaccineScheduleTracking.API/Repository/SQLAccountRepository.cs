using Microsoft.EntityFrameworkCore;
using VaccineScheduleTracking.API.Data;
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

        public async Task<Account?> GetAccountByUsernameAsync(string username)
        {
            return await dbContext.Accounts.Include(x => x.Parent).FirstOrDefaultAsync(user => user.Username == username);
        }
    }
}
