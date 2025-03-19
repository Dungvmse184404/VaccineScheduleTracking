using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API.Repository.Notifications;
using VaccineScheduleTracking.API_Test.Configurations;
using VaccineScheduleTracking.API_Test.Helpers;
using VaccineScheduleTracking.API_Test.Models.DTOs;
using VaccineScheduleTracking.API_Test.Models.DTOs.Mails;
using VaccineScheduleTracking.API_Test.Models.DTOs.Notifications;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API_Test.Services.Accounts;
using VaccineScheduleTracking.API_Test.Services.Appointments;
using static VaccineScheduleTracking.API_Test.Helpers.ValidationHelper;

namespace VaccineScheduleTracking.API_Test.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly MailFormHelper _mailFormHelper;
        private readonly DefaultConfig _config;
        private readonly IEmailService _emailService;
        private readonly IAppointmentService _appointmentsService;
        private readonly INotificationRepository _notificationRepository;
        private readonly IAccountService _accountService;

        public NotificationService(MailFormHelper mailFormHelper, DefaultConfig config, IEmailService emailService, IAppointmentService appointmentsService, INotificationRepository notificationRepository, IAccountService accountService)
        {
            _mailFormHelper = mailFormHelper;
            _config = config;
            _emailService = emailService;
            _appointmentsService = appointmentsService;
            _notificationRepository = notificationRepository;
            _accountService = accountService;
        }

        public async Task<Notification> CreateNotification(CreateNotificationDto notification)
        {
            ValidateInput(notification, "chưa điền đủ thông tin notification");

            var notice = new Notification()
            {
                AccountID = notification.AccountID,
                CreatedDate = DateTime.Now,
                Topic = notification.Topic,
                Message = notification.Message,
                MailSentDate = notification.MailSentDate
            };
            return await _notificationRepository.AddNotification(notice);
        }

        

        public async Task<Notification> DeleteNotification(Notification notification)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Notification>> GetAllNotificationAsync()
        {
             return await _notificationRepository.GetAllNotificationAsync();
        }

        public async Task<Notification> GetNotificationByIDAsync(int notificationId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Notification>> GetPendingNotifications()
        {
            return await _notificationRepository.GetPendingNotifications();
        }

        public async Task SentMailNotificationAsync()
        {
            var notifications = await _notificationRepository.GetPendingNotifications();

            foreach (var noti in notifications)
            {
                var account = await _accountService.GetAccountByIdAsync(noti.AccountID);
                if (account != null)
                {
                    // send mail
                    //notification.MailSentDate = DateTime.Now;
                    //await _notificationRepository.UpdateNotification(notification);
                }
            }

        }

        public async Task<Notification> UpdateNotification(Notification notification)
        {
            throw new NotImplementedException();
        }

        //-------------------- Anouncement ---------------------

        public async Task<Announcement> CreateAnouncement(CreateAnnouncementDto announcement)
        {
            ValidateInput(announcement, "chưa điền đủ thông tin notification");
            var toMail = new AutoMailDto()
            {
                Subject = announcement.Topic,
                Body = announcement.Message,
                Footer = announcement.Footer
            };
            var announce = new Announcement()
            {
                ToRole = announcement.ToRole,
                CreatedDate = DateTime.Now,
                Topic = toMail.Subject,
                Message = toMail.Body,
                MailSentDate = (DateTime)announcement.MailSentDate
            };


            return await _notificationRepository.AddAnnouncement(announce);
        }

        public async Task<Announcement> DeleteAnnouncement(Announcement announcement)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Announcement>> GetAllAnnouncementsAsync()
        {
            return await _notificationRepository.GetAllAnnouncementAsync();
        }

        public async Task<List<Announcement>> GetPendingAnnouncements()
        {
            return await _notificationRepository.GetPendingAnnouncements();
        }


        //-------------------- Auto Anouncement ---------------------

        public async Task<List<Appointment>> GetPendingAppointmentsAsync(int beforeDueDate)
        {
            return await _appointmentsService.GetPendingAppointments(beforeDueDate);
        }

        public async Task SentAutoMailAnnouncementAsync()
        {
            var appointments = await GetPendingAppointmentsAsync(_config.MailDueDate);
            foreach (var appointment in appointments)
            {
                var rep = await GetAutoAnnounceAppointmentIDAsync(appointment.AppointmentID);
                if (rep != null)
                    continue;

                var account = await _accountService.GetParentByChildIDAsync(appointment.ChildID);
                if (account != null)
                {
                    var mail = await _mailFormHelper.CreateReminderMailForm(appointment);
                    await _emailService.SendEmailAsync(account.Email, mail.Subject, mail.Body);
                    await CreateAutoAnnouncementMarkerAsync(appointment.AppointmentID);
                }
            }
        }


        public async Task CreateAutoAnnouncementMarkerAsync(int appointmentId)
        {
            await _notificationRepository.AddAutoAnnouncementAsync(new AutoAnnouncement()
            {
                AppointmentID = appointmentId
            });
        }

        public async Task<AutoAnnouncement> GetAutoAnnounceAppointmentIDAsync(int appointmentId)
        {
            return await _notificationRepository.GetAutoAnnounceAppointmentIDAsync(appointmentId);
        }

        public async Task<AnnouncementRecipient> GetRecipientByAppointmentIDAsync(int appointmentID)
        {
            return await _notificationRepository.GetRecipientByAppointmentIDAsync(appointmentID);
        }
        
        private async Task CreateAnnouncementRecipientAsync(int announcementID, int appointmentID)
        {
            var announcement = new AnnouncementRecipient()
            {
                AnnouncementID = announcementID,
                AppointmentID = appointmentID
            };
            await _notificationRepository.AddAnnouncementRecipientAsync(announcement);
        }

        public async Task<List<Announcement>> GetPendingToSentMailAccounts()
        {
            return await _notificationRepository.GetPendingAnnouncements();
        }


    }
}
