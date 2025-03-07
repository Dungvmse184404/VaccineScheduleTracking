using Microsoft.EntityFrameworkCore;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Appointments;

namespace VaccineScheduleTracking.API_Test.Repository.Appointments
{
    public class SQLAppointmentReopsitory : IAppointmentRepository
    {
        private readonly VaccineScheduleDbContext _dbContext;

        public SQLAppointmentReopsitory(VaccineScheduleDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            return await _dbContext.Appointments
                .Include(a => a.Child)
                .Include(a => a.Account)
                    .ThenInclude(d => d.Doctor)
                .Include(a => a.Vaccine)
                .Include(a => a.TimeSlots)
                    .ThenInclude(s => s.DailySchedule)
                .ToListAsync();
        }

        /// <summary>
        /// tổng hợp các hàm Get (Chắc cũng ko dùng nhiều)
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public async Task<List<Appointment>> SearchAppointmentByKeyword(string keyword)
        {
            if (int.TryParse(keyword, out int id))
            {
                return await GetAppointmentsByDoctorIDAsync(id)
                    ?? await GetAppointmentsByChildIDAsync(id);
            }
            return await GetAppointmentListByStatus(keyword);

        }


        /// <summary>
        /// hàm này để tìm 1 appointment thôi 
        /// </summary>
        /// <param name="id"> Cho ID zô </param>
        /// <returns> appointment có ID tương ứng </returns>
        public async Task<Appointment?> GetAppointmentByIDAsync(int id)
        {
            return await _dbContext.Appointments
                .Where(appointment => appointment.AppointmentID == id)
                .Include(a => a.Child)
                .Include(a => a.Account)
                    .ThenInclude(d => d.Doctor)
                .Include(a => a.Vaccine)
                .Include(a => a.TimeSlots)
                    .ThenInclude(s => s.DailySchedule)
                .FirstOrDefaultAsync();
        }


        /// <summary>
        /// lấy danh sách appointment hàng chờ của doctor
        /// </summary>
        /// <param name="doctorId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<Appointment>> GetPendingDoctorAppointmentAsync(int doctorId)
        {
            return await _dbContext.Appointments
                .Where(a => a.Account.Doctor.DoctorID == doctorId && a.Status == "PENDING")
                .Include(a => a.TimeSlots)
                    .ThenInclude(s => s.DailySchedule)
                .ToListAsync();
        }


        /// <summary>
        /// Nhét vô ChildID lấy ra danh sách appointment 
        /// </summary>
        /// <param name="id"> ID của Child </param>
        /// <returns> danh sách Appointment của child </returns>
        public async Task<List<Appointment>> GetAppointmentsByChildIDAsync(int id)
        {
            return await _dbContext.Appointments
               .Where(Appmt => Appmt.ChildID == id)
               .Include(a => a.Child)
               .Include(a => a.Account)
                   .ThenInclude(d => d.Doctor)
               .Include(a => a.Vaccine)
               .Include(a => a.TimeSlots)
                   .ThenInclude(s => s.DailySchedule)
               .ToListAsync();
        }


        /// <summary>
        /// hàm này nhập vào DoctorId lấy ra danh sách appointment
        /// </summary>
        /// <param name="id"> ID của Doctor </param>
        /// <returns> danh sách Appointment của Doctor </returns>
        public async Task<List<Appointment>> GetAppointmentsByDoctorIDAsync(int id)
        {
            return await _dbContext.Appointments
                .Where(a => a.Account.Doctor.DoctorID == id)
                .Include(a => a.Child)
                .Include(a => a.Account)
                   .ThenInclude(d => d.Doctor)
                .Include(a => a.Vaccine)
                .Include(a => a.TimeSlots)
                    .ThenInclude(s => s.DailySchedule)
                .ToListAsync();
        }


        public async Task<List<Appointment>> GetAppointmentListByStatus(string status)
        {
            return await _dbContext.Appointments.Where(Appmt => Appmt.Status == status).ToListAsync();
        }


        /// <summary>
        /// hàm này dùng để save appointment xuống Database
        /// </summary>
        /// <param name="appointment"></param>
        /// <returns></returns>
        public async Task<Appointment?> CreateAppointmentAsync(Appointment appointment)
        {
            await _dbContext.Appointments.AddAsync(appointment);
            await _dbContext.SaveChangesAsync();

            return appointment;
        }



        public async Task<Appointment?> UpdateAppointmentAsync(Appointment appointmentDto)
        {
            var appointment = await GetAppointmentByIDAsync(appointmentDto.AppointmentID);
            if (appointment == null)
            {
                return null;
            }
            appointment.ChildID = appointmentDto.ChildID;
            appointment.AccountID = appointmentDto.AccountID;
            appointment.VaccineID = appointmentDto.VaccineID;
            appointment.TimeSlotID = appointmentDto.TimeSlotID;
            appointment.Status = appointmentDto.Status;

            await _dbContext.SaveChangesAsync();

            return appointment;
        }

        public async Task<Appointment?> DeleteAppointmentAsync(Appointment appointment)
        {
            _dbContext.Remove(appointment);
            await _dbContext.SaveChangesAsync();

            return appointment;
        }

        public async Task<Appointment?> ChangeAppointmentStatus(Appointment appointment, string status)
        {
            appointment.Status = status;
            await _dbContext.SaveChangesAsync();

            return appointment;
        }


    }
}
