using E_Greetings.Models;
using E_Greetings.Models.DTOs;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace E_Greetings.Core.Services
{
    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(EmailDTO dto)
        {
            var client = new SmtpClient(_settings.Host)
            {
                Port = _settings.Port,
                Credentials = new NetworkCredential(_settings.Username, _settings.Password),
                EnableSsl = _settings.EnableSSL
            };

            var mail = new MailMessage
            {
                From = new MailAddress(_settings.Username),
                Subject = dto.Subject,
                Body = dto.Body,
                IsBodyHtml = true
            };

            mail.To.Add(dto.To);

            await client.SendMailAsync(mail);
        }
    }

}
