using Microsoft.Identity.Client;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Services.Accounts;
using static VaccineScheduleTracking.API_Test.Helpers.ValidationHelper;
using static VaccineScheduleTracking.API_Test.Helpers.ExceptionHelper;
using VaccineScheduleTracking.API_Test.Repository.Doctors;
using VaccineScheduleTracking.API_Test.Services.Doctors;
using System.Numerics;
using VaccineScheduleTracking.API_Test.Helpers;
using System.Net.WebSockets;
using VaccineScheduleTracking.API_Test.Repository.Staffs;
using VaccineScheduleTracking.API_Test.Configurations;
using Microsoft.AspNetCore.Identity;
namespace VaccineScheduleTracking.API_Test.Services.Staffs
{
    public class StaffService : IStaffService
    {
        private readonly TimeSlotHelper _timeSlotHelper;
        private readonly AdminAccountConfig _config;
        private readonly IPasswordHasher<Account> _passHasher;
        private readonly IStaffRepository _staffRepository;
        private readonly IDoctorServices _doctorService;
        private readonly IAccountService _accountService;

        public StaffService(TimeSlotHelper timeSlotHelper, AdminAccountConfig config, IPasswordHasher<Account> passHasher, IStaffRepository staffRepository, IDoctorServices doctorService, IAccountService accountService)
        {
            _timeSlotHelper = timeSlotHelper;
            _config = config;
            _passHasher = passHasher;
            _staffRepository = staffRepository;
            _doctorService = doctorService;
            _accountService = accountService;
        }

        private void checkRole(Account account)
        {
            if (account.Staff != null)
            {
                throw new Exception("tài khoản này đã có role Staff, hãy tạo tài khoản mới");
            }
            if (account.Doctor != null)
            {
                throw new Exception("tài khoản này đã có role Doctor, hãy tạo tài khoản mới");
            }
            if (account.Parent != null)
            {
                throw new Exception("tài khoản này đã có role Parent, hãy tạo tài khoản mới");
            }
        }

        //public async Task<Account> 

        public async Task<Account> PromoteToDoctorAsync(int accountId, string schedule)
        {
            ValidateInput(accountId, "chưa nhập accountId");
            ValidateInput(schedule, "chưa nhập lịch làm việc");
            var account = await _accountService.GetAccountByIdAsync(accountId);
            ValidateInput(account, "không tìm thấy Account");
            checkRole(account);

            account = await _doctorService.AddDoctorByAccountIdAsync(account, schedule);

            var doctorList = new List<Doctor>() { account.Doctor };
            await _doctorService.DeleteDoctorTimeSlotAsync(account.Doctor.DoctorID);
            await _doctorService.GenerateDoctorCalanderAsync(doctorList, _timeSlotHelper.SetCalendarDate());

            return account;
        }


        public async Task<Account> PromoteToStaffAsync(int accountId)
        {
            var account = await _accountService.GetAccountByIdAsync(accountId);
            ValidateInput(account, "không tìm thấy Account");

            return await AddStaffToAccountAsync(account);
        }


        public async Task<Account> AddStaffToAccountAsync(Account account)
        {
            var staff = new Staff()
            {
                AccountID = account.AccountID,
            };

            return await _staffRepository.AddStaffToAccountAsync(account, staff);
        }

        /// <summary>
        /// xác minh acc adminRecovery bằng username và password
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task RecoveryAdminAccount(Account account, string password)
        {
            if (account.Username.ToLower().Equals(_config.Username.ToLower()))
            {
                if (_passHasher.VerifyHashedPassword(account, password, _config.Password) == PasswordVerificationResult.Success)
                {
                    await AddStaffToAccountAsync(account);
                }
            }
        }
    }
}
