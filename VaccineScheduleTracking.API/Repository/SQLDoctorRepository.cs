using Microsoft.EntityFrameworkCore;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository
{
    public class SQLDoctorRepository : IDoctorRepository
    {
        private readonly VaccineScheduleDbContext _dbContext;

        public SQLDoctorRepository(VaccineScheduleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Doctor>> GetAllDoctorAsync()
        {
            return await _dbContext.Doctors.ToListAsync();
        }

        public async Task<List<DoctorTimeSlot>> GetDoctorTimeSlotsForDayAsync(int doctorID, int dailyScheduleID)
        {
            return await _dbContext.DoctorTimeSlots
                .Where(Ds => Ds.DoctorID == doctorID && Ds.DailyScheduleID == dailyScheduleID)
                .ToListAsync();
        }
        public async Task<Doctor?> GetDoctorByIDAsync(int doctorID)
        {
            return await _dbContext.Doctors
                .Include(d => d.Account)
                .FirstOrDefaultAsync(d => d.DoctorID == doctorID);
        }

        public async Task AddTimeSlotForDoctorAsync(DoctorTimeSlot doctorSlot)
        {
            _dbContext.DoctorTimeSlots.Add(doctorSlot);
            await _dbContext.SaveChangesAsync();
        }

        public Task<Doctor?> GetSuitableDoctor(int slot, DateTime time)
        {
            throw new NotImplementedException();
        }
    }
}
