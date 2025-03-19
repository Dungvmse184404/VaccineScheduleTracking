using Microsoft.EntityFrameworkCore;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API_Test.Payments.VnPay.Models;

namespace VaccineScheduleTracking.API.Data
{
    public class VaccineScheduleDbContext : DbContext
    {
        public VaccineScheduleDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<AutoAnnouncement> AutoAnnouncements { get; set; }
        public DbSet<AnnouncementRecipient> AnnouncementRecipients { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<AccountNotation> AccountNotations { get; set; }
        public DbSet<CancelAppointment> CancelAppointments { get; set; }
        public DbSet<ChildTimeSlot> ChildTimeSlots { get; set; }
        public DbSet<DoctorTimeSlot> DoctorTimeSlots { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<DailySchedule> DailySchedule { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Child> Children { get; set; }
        public DbSet<VaccineType> VaccineTypes { get; set; }
        public DbSet<Vaccine> Vaccines { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<VaccineRecord> VaccineRecords { get; set; }
        public DbSet<VaccineCombo> VaccineCombos { get; set; }
        public DbSet<VaccineContainer> VaccineContainers { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<VnPayTransaction> VnPayTransactions { get; set; }
    }
}
