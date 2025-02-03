using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Repository
{
    public interface IChildRepository
    {
        Task<List<Child>> GetChildrenByParentID(int parentID);
        Task<Child?> GetChildById(int id);
        Task<Child> AddChild(Child child);
        Task<Child> UpdateChild(int id, Child updateChild);
    }
}
