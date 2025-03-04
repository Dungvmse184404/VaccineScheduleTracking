using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using static VaccineScheduleTracking.API_Test.Helpers.TimeSlotHelper;
using static VaccineScheduleTracking.API_Test.Helpers.ExceptionHelper;
using VaccineScheduleTracking.API_Test.Services.Appointments;
using VaccineScheduleTracking.API_Test.Services.Children;
using VaccineScheduleTracking.API_Test.Services.DailyTimeSlots;
using VaccineScheduleTracking.API_Test.Services.Doctors;

public class ScheduledTaskService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ScheduledTaskService> _logger;

    public ScheduledTaskService(IServiceProvider serviceProvider, ILogger<ScheduledTaskService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var timeSlotServices = scope.ServiceProvider.GetRequiredService<ITimeSlotServices>();
                    var doctorServices = scope.ServiceProvider.GetRequiredService<IDoctorServices>();
                    var childServices = scope.ServiceProvider.GetRequiredService<IChildService>();
                    var appointmentServices = scope.ServiceProvider.GetRequiredService<IAppointmentService>();

                    int days = 7;

                    /// Tạo lịch (TimeSlot)
                    await timeSlotServices.GenerateCalanderAsync(SetCalanderDate());
                    /// Tạo lịch làm việc cho bác sĩ
                    var doctorList = await doctorServices.GetAllDoctorAsync();
                    await doctorServices.GenerateDoctorCalanderAsync(doctorList, SetCalanderDate());

                    /// Set false cho những TimeSlot trước ngày hôm nay
                    await timeSlotServices.SetOverdueTimeSlotAsync();
                    await doctorServices.SetOverdueDoctorScheduleAsync();
                    await childServices.SetOverdueChildScheduleAsync();
                    await appointmentServices.SetOverdueAppointmentAsync();
                }
                _logger.LogInformation($"ScheduledTaskService chạy lúc: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                Console.WriteLine($"ScheduledTaskService chạy lúc: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            }
            catch (Exception ex)
            {
                HandleException(ex);
                //WriteLog($"Lỗi: {ex.Message}");
            }

            // Chờ 45 phút trước khi chạy lại
            await Task.Delay(TimeSpan.FromMinutes(45), stoppingToken);
        }
    }

    private void HandleException(Exception ex)
    {
        Console.WriteLine($"Lỗi: {ex.Message}");
    }
}
