using Microsoft.EntityFrameworkCore;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository.Children
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
        public async Task<List<Child>> GetAllChildrenAsync()
        {
            return await dbContext.Children.ToListAsync();
        }

        public async Task<Child?> GetChildByID(int id)
        {
            return await dbContext.Children.FirstOrDefaultAsync(x => x.ChildID == id);
        }
        public async Task<Child> AddChild(Child child)
        {
            await dbContext.Children.AddAsync(child);
            await dbContext.SaveChangesAsync();

            return child;
        }

        public async Task<Child> UpdateChild(int id, Child updateChild)
        {
            var child = await GetChildByID(id);
            if (child == null)
            {
                throw new Exception($"Can't find Child with id {id}!");
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

        public async Task<Child> DeleteChildAsync(Child child)
        {
            if (child == null)
            {
                return null;
            }

            dbContext.Children.Remove(child);
            await dbContext.SaveChangesAsync();
            return child;
        }

        //-----------------childTimeSlot-------------------


        public Task<List<ChildTimeSlot>> GetChildTimeSlotsForDayAsync(int childID, DateOnly appointmentDate)
        {
            return dbContext.ChildTimeSlots
                .Include(x => x.DailySchedule)
                .Where(x => x.ChildID == childID && x.DailySchedule.AppointmentDate == appointmentDate)
                .ToListAsync();
        }

        public Task<ChildTimeSlot> GetChildTimeSlotsByID(int id)
        {
            return dbContext.ChildTimeSlots.FirstOrDefaultAsync(x => x.ChildTimeSlotID == id);
        }

        public async Task<ChildTimeSlot?> GetChildTimeSlotBySlotNumberAsync(int slotNumber, DateOnly date)
        {
            return await dbContext.ChildTimeSlots.FirstOrDefaultAsync(x => x.SlotNumber == slotNumber && x.DailySchedule.AppointmentDate == date);
        }

        public async Task<ChildTimeSlot> AddChildTimeSlotAsync(ChildTimeSlot childTimeSlot)
        {
            await dbContext.ChildTimeSlots.AddAsync(childTimeSlot);
            await dbContext.SaveChangesAsync();
            return childTimeSlot;
        }


        public async Task UpdateChildTimeSlotsAsync(ChildTimeSlot slot)
        {

            var childTimeSlot = await GetChildTimeSlotsByID(slot.ChildTimeSlotID);
            if (childTimeSlot == null)
            {
                throw new Exception($"Can't find TimeSlotID {slot.ChildTimeSlotID} for child {slot.ChildID}!");
            }
            slot.ChildID = childTimeSlot.ChildID;
            slot.SlotNumber = childTimeSlot.SlotNumber;
            slot.Available = childTimeSlot.Available;
            slot.DailyScheduleID = childTimeSlot.DailyScheduleID;

            await dbContext.SaveChangesAsync();
        }
    }
}
