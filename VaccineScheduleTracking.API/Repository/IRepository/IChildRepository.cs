using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository.IRepository
{
    public interface IChildRepository
    {
        Task<List<Child>> GetChildrenByParentID(int parentID);
        Task<Child?> GetChildById(int id);
        Task<Child> AddChild(Child child);
        Task<Child> UpdateChild(int id, Child updateChild);
        Task<Child> DeleteChildAsync(Child child);
    }
}
