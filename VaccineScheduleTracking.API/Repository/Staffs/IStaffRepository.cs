using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository.Staffs
{
    public interface IStaffRepository
    {
        Task<Account> AddStaffToAccountAsync(Account account, Staff staffInfo);
        Task<Account> AddManagerToAccountAsync(Account account, Manager manager);
    }
}
