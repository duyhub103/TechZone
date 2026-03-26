
using System.Net;
using System.Net.Mail;

namespace MyWeb.Services
{
    public class EmailSender
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

            string host = emailSettings["Host"] ?? "smtp.gmail.com";
            int port = int.Parse(emailSettings["Port"] ?? "587");
            string fromEmail = emailSettings["Email"] ?? "";
            string password = emailSettings["Password"] ?? ""; //app pwd của gg

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
