using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Services
{
    public interface IChildService
    {
        Task<List<Child>> GetParentChildren(int parentID); 
    }
}
