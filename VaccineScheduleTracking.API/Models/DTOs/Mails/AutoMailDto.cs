using System;
using System.Text.RegularExpressions;


namespace VaccineScheduleTracking.API_Test.Models.DTOs.Mails
{
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
                return $@"
            <div style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: auto; padding: 20px; 
                        border: 1px solid #ddd; border-radius: 10px; background-color: #fff;'>
                <h2 style='color: #007bff;'>Xin chào {RecipientName},</h2>
                <p>{ConvertToHtml(_body)}</p>
                <hr style='border-top: 1px solid #ddd; margin: 20px 0;'>
                {Footer}
            </div>";
            }
            set => _body = value;
        }

        public string Footer
        {
            get => string.IsNullOrEmpty(_footer) ? DefaultFooter : WrapFooter(_footer);
            set => _footer = value;
        }

        private string DefaultFooter => @"
            <div style='text-align: center; font-size: 14px; color: #777;'>
                <strong>Trân trọng,</strong><br>Đội ngũ hỗ trợ
            </div>";

        private string WrapFooter(string customFooter)
        {
            return $@"
            <div style='text-align: center; font-size: 14px; color: #777;'>
                {ConvertToHtml(customFooter)}
            </div>";
        }

        private string ConvertToHtml(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            text = text.Replace("\n", "<br>").Replace(Environment.NewLine, "<br>");

            text = Regex.Replace(text, @"\|(.*?)\|", "<strong>$1</strong>", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            text = Regex.Replace(text, @"\[(.*?)\]", "<span style='color: gray;'>$1</span>", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            return text.Trim();
        }

        public string ToHtml()
        {
            return $@"
            <div style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: auto; padding: 20px;'>
                {Body}
            </div>".Trim();
        }
    }



}
