using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using VaccineScheduleTracking.API_Test.Services.Appointments;
using VaccineScheduleTracking.API_Test.Services.DailyTimeSlots;
using VaccineScheduleTracking.API_Test.Repository;
using VaccineScheduleTracking.API.Services;
using VaccineScheduleTracking.API_Test.Services.Children;


namespace VaccineScheduleTracking.API_Test.Services
{
    public class StartupServices : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public StartupServices(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var timeSlotServices = services.GetRequiredService<ITimeSlotServices>();
                var doctorServices = services.GetRequiredService<IDoctorServices>();
                var childServices = services.GetRequiredService<IChildService>();
                var appointmentServices = services.GetRequiredService<IAppointmentService>();

                int days = 6;
                /// tạo lịch (TimeSlot)
                await timeSlotServices.GenerateCalanderAsync(days);
                /// tạo lịch làm vc cho bác sĩ
                var doctorList = await doctorServices.GetAllDoctorAsync();
                await doctorServices.GenerateDoctorCalanderAsync(doctorList, days);
                /// set false cho những timeSlot trước ngày hôm nay
                await timeSlotServices.SetOverdueTimeSlotAsync();
                await doctorServices.SetOverdueDoctorScheduleAsync();
                await childServices.SetOverdueChildScheduleAsync();
                await appointmentServices.SetOverdueAppointmentAsync();
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += $"\nInner Exception: {ex.InnerException.Message}";
                }
                Console.WriteLine($"An error occurred during startup: {errorMessage}");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}

