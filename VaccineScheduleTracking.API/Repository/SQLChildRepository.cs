using Microsoft.EntityFrameworkCore;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Repository
{
    public class SQLChildRepository : IChildRepository
    {
        private readonly VaccineScheduleDbContext dbContext;

        public SQLChildRepository(VaccineScheduleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Child>> GetChildrenByParentID(int parentID)
        {
            return await dbContext.Children.AsQueryable().Where(x => x.ParentID == parentID).ToListAsync();
        }
    }
}
