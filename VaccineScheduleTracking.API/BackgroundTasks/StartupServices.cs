using static VaccineScheduleTracking.API_Test.Helpers.ExceptionHelper;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using VaccineScheduleTracking.API_Test.Services.Appointments;
using VaccineScheduleTracking.API_Test.Services.DailyTimeSlots;
using VaccineScheduleTracking.API_Test.Repository;
using VaccineScheduleTracking.API_Test.Services.Children;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using VaccineScheduleTracking.API_Test.Services.Doctors;

namespace VaccineScheduleTracking.API_Test.BackgroundTasks
{
    public class StartupServices : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(45);

        public StartupServices(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var services = scope.ServiceProvider;

                try
                {
                    var timeSlotServices = services.GetRequiredService<ITimeSlotServices>();
                    var doctorServices = services.GetRequiredService<IDoctorServices>();
                    var childServices = services.GetRequiredService<IChildService>();
                    var appointmentServices = services.GetRequiredService<IAppointmentService>();

                    int days = 180;
                    /// tạo lịch (TimeSlot)
                    await timeSlotServices.GenerateCalanderAsync(days);
                    /// tạo lịch làm vc cho bác sĩ
                    var docAccountList = await doctorServices.GetAllDoctorAsync();
                    var doctorList = docAccountList.Select(docAccount => docAccount.Doctor).ToList();

                    await doctorServices.GenerateDoctorCalanderAsync(doctorList, days);
                    /// set false cho những timeSlot trước ngày hôm nay
                    await timeSlotServices.SetOverdueTimeSlotAsync();
                    await doctorServices.SetOverdueDoctorScheduleAsync();
                    await childServices.SetOverdueChildScheduleAsync();
                    await appointmentServices.SetOverdueAppointmentAsync();
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            // Perform any necessary cleanup here
            await base.StopAsync(stoppingToken);
        }
    }
}
