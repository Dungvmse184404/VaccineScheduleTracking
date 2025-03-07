using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API.Models.Entities;
using static VaccineScheduleTracking.API_Test.Helpers.TimeSlotHelper;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API_Test.Helpers;
using Microsoft.EntityFrameworkCore.Internal;

namespace VaccineScheduleTracking.API_Test.Repository.Doctors
{
    public class SQLDoctorRepository : IDoctorRepository
    {
        private readonly TimeSlotHelper _timeSlotHelper;
        private readonly VaccineScheduleDbContext _dbContext;

        public SQLDoctorRepository(TimeSlotHelper timeSlotHelper, VaccineScheduleDbContext dbContext)
        {
            _timeSlotHelper = timeSlotHelper;
            _dbContext = dbContext;
        }

        // ----------------- hàm tạm để sửa lỗi ----------------- 

        public async Task<Account> GetAccountByAccountIDAsync(int accountId)
        {
            return await _dbContext.Accounts
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AccountID == accountId);
        }
        public async Task<Doctor?> GetDoctorByAccountIDAsync(int accountId) 
        {
            return await _dbContext.Doctors
                .Include(a => a.Account)
                .FirstOrDefaultAsync(a => a.AccountID == accountId);
        }
        //-----------------  -----------------  ------------------

        public async Task<List<Account>> GetAllDoctorAsync()
        {
            return await _dbContext.Accounts
                .Include(a => a.Doctor)
                .Where(a => a.Doctor != null)
                .ToListAsync();
        }


        public async Task<Account?> GetDoctorByIDAsync(int doctorID)
        {
            return await _dbContext.Accounts
                .Include(d => d.Doctor)
                .Where(d => d.Doctor.DoctorID  == doctorID)
                .FirstOrDefaultAsync();
        }


        public async Task<Account?> UpdateDoctorAsync(Doctor doctor)
        {
            var doc = await GetDoctorByIDAsync(doctor.DoctorID);
            doc.Doctor.DoctorTimeSlots = doctor.DoctorTimeSlots;
            await _dbContext.SaveChangesAsync();

            return doc;
        }

        //------------------------doctorTimeSlot-----------------------

        public async Task<DoctorTimeSlot?> GetSpecificDoctorTimeSlotAsync(int doctorID, DateOnly date, int slot)
        {
            return await _dbContext.DoctorTimeSlots
                .FirstOrDefaultAsync(Ds => Ds.DoctorID == doctorID && Ds.DailySchedule.AppointmentDate == date && Ds.SlotNumber == slot);
        }



        public async Task<List<DoctorTimeSlot>> GetDoctorTimeSlotsForDayAsync(int doctorID, DateOnly date)
        {
            var timeSlots = await _dbContext.DoctorTimeSlots
                .Where(Ds => Ds.DoctorID == doctorID && Ds.DailySchedule.AppointmentDate == date)
                .ToListAsync();

            return timeSlots;
        }
        public async Task AddTimeSlotForDoctorAsync(DoctorTimeSlot doctorSlot)
        {
            _dbContext.DoctorTimeSlots.Add(doctorSlot);
            await _dbContext.SaveChangesAsync();
        }

        //public Task<Doctor?> GetSuitableDoctor(int slot, DateTime time)
        //{
        //    throw new NotImplementedException();
        //}



        public async Task<DoctorTimeSlot> GetDoctorTimeSlotByIDAsync(int doctorTimeSlotID)
        {
            return await _dbContext.DoctorTimeSlots
                .Where(ts => ts.DoctorTimeSlotID == doctorTimeSlotID)
                .FirstOrDefaultAsync();
        }

        public async Task<DoctorTimeSlot> UpdateDoctorTimeSlotAsync(DoctorTimeSlot doctorSlot)
        {
            var slot = await GetDoctorTimeSlotByIDAsync(doctorSlot.DoctorTimeSlotID);

            slot.DoctorID = doctorSlot.DoctorID;
            slot.SlotNumber = doctorSlot.SlotNumber;
            slot.Available = doctorSlot.Available;
            slot.DailyScheduleID = doctorSlot.DailyScheduleID;

            await _dbContext.SaveChangesAsync();

            return slot;
        }

        public async Task DeleteDoctorTimeSlotByDoctorIDAsync(int doctorId)
        {
            var currentSlotNumber = _timeSlotHelper.CalculateSlotNumber(TimeOnly.FromDateTime(DateTime.Now));

            var timeSlots = await _dbContext.DoctorTimeSlots
                .Where(ts => ts.DoctorID == doctorId &&
                             (ts.DailySchedule.AppointmentDate > DateOnly.FromDateTime(DateTime.Today) ||
                              (ts.DailySchedule.AppointmentDate == DateOnly.FromDateTime(DateTime.Today) &&
                               ts.SlotNumber >= currentSlotNumber)))
                .ToListAsync();

            if (timeSlots.Any())
            {
                _dbContext.DoctorTimeSlots.RemoveRange(timeSlots);
                await _dbContext.SaveChangesAsync();
            }
        }



    }
}
