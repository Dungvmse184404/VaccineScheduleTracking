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

        public async Task<Child> AddChild(Child child)
        {
            await dbContext.Children.AddAsync(child);
            await dbContext.SaveChangesAsync();

            return child;
        }

        public async Task<Child?> GetChildById(int id)
        {
            return await dbContext.Children.FirstOrDefaultAsync(x => x.ChildID == id);
        }

        public async Task<Child> UpdateChild(int id, Child updateChild)
        {
            var child = await GetChildById(id);
            if (child == null)
            {
                throw new Exception("This child is not exist!");
            }
            child.Firstname = updateChild.Firstname;
            child.Lastname = updateChild.Lastname;
            child.Weight = updateChild.Weight;
            child.Height = updateChild.Height;
            child.DateOfBirth = updateChild.DateOfBirth;
            child.Gender = updateChild.Gender;

            await dbContext.SaveChangesAsync();
            return child;
        }

        public async Task<Child> DeleteChildAsync(int id)
        {
            var child = await GetChildById(id);
            if (child != null)
            {
                dbContext.Children.Remove(child);
                await dbContext.SaveChangesAsync();
            }
            return child;   
        }

        public Task<Child> DeleteChild(int id)
        {
            throw new NotImplementedException();
        }
    } 
}
