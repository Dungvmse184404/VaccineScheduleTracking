using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Services.Staffs
{
    public interface IStaffService
    {
        Task<Account> PromoteToDoctorAsync(int accointId, string schedule);
        Task<Account> PromoteToStaffAsync(int accountId);
        Task<Account> PromoteToManagerAsync(int accountId);
        Task<Account> AddStaffToAccountAsync(Account account);
        Task RecoveryAdminAccount(Account account, string password);
    }
}
