using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository.Children
{
    public interface IChildRepository
    {
        Task<List<Child>> GetChildrenByParentID(int parentID);
        Task<List<Child>> GetAllChildrenAsync();
        Task<Child?> GetChildByID(int id);
        Task<Child> AddChild(Child child);
        Task<Child> UpdateChild(int id, Child updateChild);
        Task<Child> DisableChildAsync(Child child);

        //-----------------ChildTimeSlot-----------------
        Task<ChildTimeSlot?> GetChildTimeSlotBySlotNumberAsync(int childId, int slotNumber, DateOnly date);
        Task<ChildTimeSlot?> GetChildTimeSlotByIDAsync(int childTimSlotId);
        Task<ChildTimeSlot> AddChildTimeSlotAsync(ChildTimeSlot childTimeSlot);
        Task<List<ChildTimeSlot>> GetChildTimeSlotsForDayAsync(int childID, DateOnly appointmentDate);
        Task<List<ChildTimeSlot>> GetAllTimeSlotByChildIDAsync(int childId);
        Task<ChildTimeSlot> UpdateChildTimeSlotsAsync(ChildTimeSlot slot);
        Task DeleteChildTimeSlotsAsync(List<ChildTimeSlot> slot);
    }
}
