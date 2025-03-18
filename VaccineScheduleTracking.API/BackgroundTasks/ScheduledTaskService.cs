using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using VaccineScheduleTracking.API_Test.Services.Appointments;
using VaccineScheduleTracking.API_Test.Services.Children;
using VaccineScheduleTracking.API_Test.Services.DailyTimeSlots;
using VaccineScheduleTracking.API_Test.Services.Doctors;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Helpers;
using VaccineScheduleTracking.API_Test.Configurations;

public class ScheduledTaskService : BackgroundService
{
    private readonly TimeSlotHelper _timeHelper;
    private readonly DefaultConfig _config;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ScheduledTaskService> _logger;

    public ScheduledTaskService(TimeSlotHelper timeHelper, DefaultConfig config, IServiceProvider serviceProvider, ILogger<ScheduledTaskService> logger)
    {
        _timeHelper = timeHelper;
        _config = config;
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

                    /// Tạo lịch (TimeSlot)
                    await timeSlotServices.GenerateCalanderAsync(_timeHelper.SetCalendarDate());
                    /// Tạo lịch làm việc cho bác sĩ
                    var docAccountList = await doctorServices.GetAllDoctorAsync();
                    var doctorList = docAccountList.Select(docAccount => docAccount.Doctor).ToList();


                    await doctorServices.GenerateDoctorCalanderAsync(doctorList, _timeHelper.SetCalendarDate());

                    /// Set false cho những TimeSlot trước ngày hôm nay
                    await timeSlotServices.SetOverdueTimeSlotAsync(_config.OverdueSchedule);
                    await doctorServices.SetOverdueDoctorScheduleAsync(_config.OverdueSchedule);
                    await childServices.SetOverdueChildScheduleAsync(_config.OverdueSchedule);
                    await appointmentServices.SetOverdueAppointmentAsync(_config.OverdueSchedule);
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
