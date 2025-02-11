using Microsoft.EntityFrameworkCore;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Repository
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

        public async Task<List<int>> GetUsedSlots(int doctorId, DateTime Date)
        {
            var appointments = await _dbContext.Appointments
                .Where(a => a.DoctorID == doctorId && a.Time == Date)
                .ToListAsync();

            var usedSlots = appointments
                .Select(a => a.Slot)
                .Distinct()
                .ToList();

            return usedSlots;
        }



        public async Task<Doctor?> GetSuitableDoctor(int slot, DateTime time)
        {
            var suitableDoctor = await _dbContext.Doctors
                 //.Where(d => d.Contains(slot)) // Bác sĩ có slot trong danh sách
                 .FirstOrDefaultAsync(d =>
                     _dbContext.DailySchedule.Any(s => s.DailyScheduleID == slot && s.Appointment.Time.Date == time.Date && s.AppointmentID == null) // Kiểm tra slot có trống trong ngày không
                 );

                return suitableDoctor; 
        }
    }
}
