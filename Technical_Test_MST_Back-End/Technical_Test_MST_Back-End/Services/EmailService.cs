using MimeKit;
using MailKit.Net.Smtp;

namespace Technical_Test_MST_Back_End.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendResetTokenEmail(string toEmail, string resetToken)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:FromEmail"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = "Password Reset";
            email.Body = new TextPart("plain")
            {
                Text = $"Use this token to reset your password: {resetToken}"
            };

            using (var smtp = new SmtpClient())
            {
                smtp.Connect(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:Port"]), false);
                smtp.Authenticate(_configuration["EmailSettings:FromEmail"], _configuration["EmailSettings:Password"]);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }
    }
}
