
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace BookCARTWebApi.Repositories
{
    public class EmailRepo : IEmailRepo
    {
        private readonly IConfiguration _configuration;

        public EmailRepo(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["SmtpSettings:Gmail:SenderEmail"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };

            email.Body = bodyBuilder.ToMessageBody();

            var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync(_configuration["SmtpSettings:Gmail:Server"], int.Parse(_configuration["SmtpSettings:Gmail:Port"]), SecureSocketOptions.StartTls);
            await smtpClient.AuthenticateAsync(_configuration["SmtpSettings:Gmail:Username"], _configuration["SmtpSettings:Gmail:Password"]);
            await smtpClient.SendAsync(email);
            await smtpClient.DisconnectAsync(true);
        }
    }
}
