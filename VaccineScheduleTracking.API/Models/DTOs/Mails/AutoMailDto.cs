using System;
using System.Text.RegularExpressions;


namespace VaccineScheduleTracking.API_Test.Models.DTOs.Mails
{
    using System.Text.RegularExpressions;

    public class AutoMailDto
    {
        public string RecipientName { get; set; }
        private string _subject;
        private string _body;
        private string _footer;

        public string Subject
        {
            get => ConvertToHtml(_subject);
            set => _subject = value;
        }

        public string Body
        {
            get
            {
                string footerContent = string.IsNullOrEmpty(_footer) ? "<p>Trân trọng, <br> Đội ngũ hỗ trợ </p>" : _footer;
                return ConvertToHtml(_body) + "<br><hr style='border-top: 1px solid #ddd; margin: 20px 0;'><br>" + ConvertToHtml(footerContent);
            }
            set => _body = value;
        }

        public string Footer
        {
            set => _footer = value;
        }

        private string ConvertToHtml(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            text = text.Replace("\n", "<br>").Replace(Environment.NewLine, "<br>");
            text = Regex.Replace(text, @"\|(.*?)\|", "<strong>$1</strong>");

            return text;
        }

        public string ToHtml()
        {
            return $@"
    <div style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px;'>
        <h2 style='color: #007bff;'>Kính gửi {RecipientName},</h2>
        <p>{Body}</p>
    </div>";
        }
    }



}
