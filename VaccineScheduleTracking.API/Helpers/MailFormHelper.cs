using System.Data;
using System.Globalization;
using System.Text;
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
                Subject = "Nhắc nhở lịch tiêm chủng",
                Body = $@"
                    Trung tâm tiêm chủng xin thông báo rằng bé <strong>{childName}</strong>, có lịch hẹn tiêm chủng sắp tới.
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

        public async Task<AutoMailDto> CreateRoleAssignmentMail(string accountName, string role)
        {
            return new AutoMailDto
            {
                Footer = "Trân trọng,<br>Đội ngũ quản trị viên",
                RecipientName = accountName,
                Subject = "Thông báo cấp quyền truy cập",
                Body = $@"
                    Trung tâm tiêm chủng xin thông báo rằng tài khoản của bạn đã được cấp quyền hạn mới trên hệ thống |Vaccine Schedule Tracking System|.<br>
                    |Vai trò mới:| {role} 📜<br>
                    hiện tại tài khoản trên đã được mở khóa các tính năng và quyền truy cập tương ứng trên hệ thống.<br>
                    Nếu bạn có bất kỳ thắc mắc nào, vui lòng liên hệ với quản trị viên để được hỗ trợ.<br>
                    📞: 0772.706.420<br>
                    📧: koi221204@gmail.com<br>
                    [đây là tin nhắn tự động, vui lòng không phản hồi]"
            };
        }


        public async Task<AutoMailDto> CreateComboRegisterMail(List<Appointment> appo, string comboName)
        {
            var parentAccount = await _accountService.GetParentByChildIDAsync(appo[0].ChildID);

            string parentName = $"{parentAccount.Lastname} {parentAccount.Firstname}";
            string childName = $"{appo[0].Child.Lastname} {appo[0].Child.Firstname}";

            // Chuỗi chứa lịch hẹn
            StringBuilder appointmentDetails = new StringBuilder();

            foreach (var app in appo)
            {
                string date = app.TimeSlots.DailySchedule.AppointmentDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                TimeOnly time = app.TimeSlots.StartTime;
                string vaccineName = app.Vaccine.Name;

                appointmentDetails.AppendLine($"📅 {date} - 🕒 {time} - Loại vaccine: {vaccineName} 💉<br>");
            }

            return new AutoMailDto
            {
                Footer = "Trân trọng,<br>Đội ngũ hỗ trợ",
                RecipientName = parentName,
                Subject = "Thông báo đăng kí Combo tiêm chủng",
                Body = $@"
            Trung tâm tiêm chủng xin thông báo combo |{comboName}| đã được xác nhận đăng kí cho bé |{childName}|.<br>
            <b>Lịch tiêm đã được hệ thống sắp xếp như sau:</b><br>
            {appointmentDetails}<br><br>

            Nếu bạn có bất kỳ thắc mắc nào, vui lòng liên hệ với trung tâm y tế để được hỗ trợ.<br>
            📞: 0772.706.420<br>
            📧: koi221204@gmail.com<br>
            [đây là tin nhắn tự động, vui lòng không phản hồi]"
            };
        }

    }
}