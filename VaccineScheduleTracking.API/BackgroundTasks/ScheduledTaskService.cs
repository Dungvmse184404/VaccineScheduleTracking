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

public class ScheduledTaskService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ScheduledTaskService> _logger;
    private readonly TimeSlotHelper _timeSlotHelper;

    public ScheduledTaskService(IServiceProvider serviceProvider, ILogger<ScheduledTaskService> logger, TimeSlotHelper timeSlotHelper)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _timeSlotHelper = timeSlotHelper; 
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
                    await timeSlotServices.GenerateCalanderAsync(_timeSlotHelper.SetCalendarDate());
                    /// Tạo lịch làm việc cho bác sĩ
                    var docAccountList = await doctorServices.GetAllDoctorAsync();
                    var doctorList = docAccountList.Select(docAccount => docAccount.Doctor).ToList();


                    await doctorServices.GenerateDoctorCalanderAsync(doctorList, _timeSlotHelper.SetCalendarDate());

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
