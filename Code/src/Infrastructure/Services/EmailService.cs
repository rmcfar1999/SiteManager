using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using SiteManager.V4.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace SiteManager.V4.Infrastructure.Services
{
    public class EmailService : IEmailService, IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        private readonly string _smtpFromAddress;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _smtpHost = _configuration.GetValue<string>("AppSettings:SmtpHost");
            _smtpPort = _configuration.GetValue<int>("AppSettings:SmtpPort");
            _smtpUser = _configuration.GetValue<string>("AppSettings:SmtpUser");
            _smtpPass = _configuration.GetValue<string>("AppSettings:SmtpPass");
            _smtpFromAddress = configuration.GetValue<string>("AppSettings:SmtpFromAddress");
        }

        public async void Send(string to, string subject, string html)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_smtpFromAddress));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(_smtpHost, _smtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_smtpUser, _smtpPass);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Send(email, subject, htmlMessage);
        }
    }
}
