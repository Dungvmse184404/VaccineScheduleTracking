using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Services.Children
{
    public interface IChildService
    {
        Task<Child?> GetChildByIDAsync(int childID);
        Task<List<Child>> GetParentChildren(int parentID);
        Task<Child> AddChild(Child child);
        Task<Child> UpdateChild(int id, Child child);
        Task<Child> DeleteChild(int id);

        //-----------------ChildTimeSlot-----------------
        Task<ChildTimeSlot?> GetChildTimeSlotBySlotNumberAsync(int slotNumber, DateOnly date);
        Task<ChildTimeSlot> CreateChildTimeSlot(int slotNumber, DateOnly date, int ChildID);
        Task SetOverdueChildScheduleAsync();
    }
}
