using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository.Staffs
{
    public interface IStaffRepository
    {
        Task<Account> AddStaffToAccountAsync(Account account, Staff staffInfo);
    }
}
