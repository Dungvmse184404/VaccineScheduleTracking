using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API.Repository;

namespace VaccineScheduleTracking.API.Services
{
    public class ChildService : IChildService
    {
        private readonly IChildRepository childRepository;

        public ChildService(IChildRepository childRepository)
        {
            this.childRepository = childRepository;
        }

        public async Task<List<Child>> GetParentChildren(int parentID)
        {
            return await childRepository.GetChildrenByParentID(parentID);
        }
    }
}
