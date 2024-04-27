using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Demo.PLL.Services.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configration;

        public EmailSender(IConfiguration configration)
        {
            _configration = configration;
        }
        public async Task SendAsync(string from, string recipients, string subject, string body)
        {

            var senderEmail = _configration["EmailSetting:SenderEmail"];
            var senderPassword = _configration["EmailSetting:SenderPassword"];
            var emailMessage = new MailMessage();
            emailMessage.From = new MailAddress(from);

            emailMessage.To.Add(recipients);
            emailMessage.Subject = subject;
            emailMessage.Body = $"<html><body>{body}</body></html>";
            emailMessage.IsBodyHtml = true;
            var stmp = new SmtpClient(_configration["EmailSetting:SmtpClientServer"],
                int.Parse(_configration["EmailSetting:SmtpClientPort"]))
            {
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true
            };
            await stmp.SendMailAsync(emailMessage);


        } 
    }
}
