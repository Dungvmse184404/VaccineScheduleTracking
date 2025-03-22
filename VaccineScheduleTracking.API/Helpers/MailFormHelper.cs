using System.Globalization;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Configurations;
using VaccineScheduleTracking.API_Test.Models.DTOs.Appointments;
using VaccineScheduleTracking.API_Test.Models.DTOs.Mails;
using VaccineScheduleTracking.API_Test.Services.Accounts;

namespace VaccineScheduleTracking.API_Test.Helpers
{
    public class MailFormHelper
    {
        private readonly IAccountService _accountService;

        public MailFormHelper(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<AutoMailDto> CreateReminderMailForm(Appointment appointment)
        {
            var parentAccount = await _accountService.GetParentByChildIDAsync(appointment.ChildID);

            string parentName = $"{parentAccount.Lastname} {parentAccount.Firstname}";
            string childName = $"{appointment.Child.Lastname} {appointment.Child.Firstname}";
            string date = appointment.TimeSlots.DailySchedule.AppointmentDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            TimeOnly time = appointment.TimeSlots.StartTime;

            return new AutoMailDto()
            {
                RecipientName = parentName,
                Subject = "📢 Nhắc nhở lịch tiêm chủng 📢",
                Body = $@"
                    Chúng tôi xin thông báo rằng con của bạn, <strong>{childName}</strong>, có lịch hẹn tiêm chủng sắp tới.
                    <br>
                    📅| Ngày hẹn:| {date} <br>
                    ⏰| Giờ hẹn:| {time} <br>
                    💉| Vắc xin:| {appointment.Vaccine.Name} <br>
                    Vui lòng đưa trẻ đến đúng giờ và đem theo các loại |giấy tờ tùy thân| cần thiết cho trẻ để đảm bảo quá trình tiêm chủng diễn ra thuận lợi.
                    <br><br>
                    Nếu bạn có bất kỳ câu hỏi nào, hãy liên hệ với trung tâm y tế để được hỗ trợ.<br>
                    📞: 0772.706.420<br>
                    📧: koi221204@gmail.com<br>
                    [đây là tin nhắn tự động, vui lòng không phản hồi]"
            };
        }


    }
}