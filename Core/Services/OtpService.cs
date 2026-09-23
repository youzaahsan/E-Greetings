using E_Greetings.Core.Services;
using System;
using System.Security.Cryptography;
using E_Greetings.Models.DTOs;

namespace E_Greetings.Core.Services
{
    public class OtpService
    {
        private readonly EmailService _emailService;

        public OtpService(EmailService emailService)
        {
            _emailService = emailService;
        }
        public string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public OtpModel GenerateOtpWithExpiry()
        {
            return new OtpModel
            {
                Otp = GenerateOtp(),
                ExpiryTime = DateTime.UtcNow.AddMinutes(5)
            };
        }

        public async Task SendOtpEmail(string toEmail)
        {
            var OtpModel = GenerateOtpWithExpiry();
            var emailDto = new EmailDTO
            {
                To = toEmail,
                Subject = "Your OTP Code",
                Body = $"Your OTP code is: {OtpModel.Otp}. It will expire in 5 minutes."
            };
            await _emailService.SendEmailAsync(emailDto);
        }
    }

    public class OtpModel
    {
        public string Otp { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
