using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Repository
{
    public interface IAccountRepository 
    {
        Task<Account?> GetAccountByUsernameAsync(string username);
    }
}
