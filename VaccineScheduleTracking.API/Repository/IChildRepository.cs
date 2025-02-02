using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Repository
{
    public interface IChildRepository
    {
        Task<List<Child>> GetChildrenByParentID(int parentID);
    }
}
