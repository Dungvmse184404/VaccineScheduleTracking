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

        public async Task<List<DoctorTimeSlot>> GetDoctorTimeSlotsForDayAsync(int doctorID, DateOnly date)
        {
            var timeSlots = await _dbContext.DoctorTimeSlots
                .Where(Ds => Ds.DoctorID == doctorID && Ds.DailySchedule.AppointmentDate == date)
                .ToListAsync();

            return timeSlots;
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

        public async Task<DoctorTimeSlot> GetDoctorTimeSlotByIDAsync(int doctorTimeSlotID)
        {
            return await _dbContext.DoctorTimeSlots
                .FirstOrDefaultAsync(ts => ts.DoctorTimeSlotID == doctorTimeSlotID);
        }

        public async Task UpdateDoctorTimeSlotAsync(DoctorTimeSlot doctorSlot)
        {
            var slot = await GetDoctorTimeSlotByIDAsync(doctorSlot.DoctorTimeSlotID);

            slot.DoctorID = doctorSlot.DoctorID;
            slot.SlotNumber = doctorSlot.SlotNumber;
            slot.Available = doctorSlot.Available;
            slot.DailyScheduleID = doctorSlot.DailyScheduleID;

            await _dbContext.SaveChangesAsync();

        }
        


    }
}
