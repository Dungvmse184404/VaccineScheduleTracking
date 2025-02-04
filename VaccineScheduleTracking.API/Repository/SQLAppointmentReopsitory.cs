using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VaccineScheduleTracking.API.Repository
{
    public class SQLAppointmentReopsitory : IAppointmentRepository
    {
        private readonly VaccineScheduleDbContext _dbContext;

        public SQLAppointmentReopsitory(VaccineScheduleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Appointment?> SearchAppointmentByKeyword(string keyword)
        {
            if (int.TryParse(keyword, out int id))
            {
                return await GetAppointmentByID(id)
                    ?? await GetAppointmentByDoctorID(id)
                    ?? await GetAppointmentByChildID(id);
            }
            return await GetAppointmentByStatus(keyword);

        }
        public async Task<Appointment?> GetAppointmentByID(int id)
        {
            return await _dbContext.Appointments.FirstOrDefaultAsync(appointment => appointment.AppointmentID == id);
        }

        public async Task<Appointment?> GetAppointmentByChildID(int id)
        {
            return await _dbContext.Appointments.FirstOrDefaultAsync(appointment => appointment.ChildID == id);
        }
        public async Task<Appointment?> GetAppointmentByDoctorID(int id)
        {
            return await _dbContext.Appointments.FirstOrDefaultAsync(appointment => appointment.DoctorID == id);
        }

        public async Task<Appointment?> GetAppointmentByStatus(string status)
        {
            return await _dbContext.Appointments.FirstOrDefaultAsync(appointment => appointment.Status == status);
        }


        public async Task<Appointment?> CreateAppointmentAsync(Appointment appointment)
        {
            await _dbContext.Appointments.AddAsync(appointment);
            await _dbContext.SaveChangesAsync();

            return appointment;
        }


        public async Task<List<Appointment>> GetAllAppointmentAsync(AppointmentDto appointmentDto)
        {
            var query = _dbContext.Appointments.Include(i => i.Doctor)
                                                         .Include(i => i.Child)
                                                         .Include(i => i.VaccineType).AsQueryable();
            if (appointmentDto.AppointmentID.HasValue)
            {
                query = query.Where(i => i.AppointmentID == appointmentDto.AppointmentID);
            }
            if (appointmentDto.Time.HasValue)
            {
                query = query.Where(i => i.Time == appointmentDto.Time);
            }
            if (!string.IsNullOrEmpty(appointmentDto.Status))
            {
                query = query.Where(i => i.Status == appointmentDto.Status);
            }

            return await query.ToListAsync();
        }

        public async Task<Appointment?> ModifyAppointmentAsync(AppointmentDto appointmentDto)
        {
            var appointment = await SearchAppointmentByKeyword(appointmentDto.AppointmentID?.ToString());
            if (appointment == null)
            {
                return null;
            }
            appointment.ChildID = (int)appointmentDto.ChildID;
            appointment.DoctorID = (int)appointmentDto.DoctorID;
            appointment.VaccineTypeID = (int)appointmentDto.VaccineTypeID;
            appointment.Time = (DateTime)appointmentDto.Time;
            appointment.Status = appointmentDto.Status;

            await _dbContext .SaveChangesAsync();

            return appointment;
        }
    }
}
