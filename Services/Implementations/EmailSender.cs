using MyWeb.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace MyWeb.Services.Implementations
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // lấy cấu hình từ appsettings.json
            var emailSettings = _configuration.GetSection("EmailSettings");

            string host = "smtp.gmaail.com";
            int port = 587;
            string fromEmail = "your-email@gmail.com";
            string password = "your-app-password"; //app pwd của gg

            // Setup Client
            using var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail, "TechZone Store"),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);

        }

    }
}
