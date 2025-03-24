using System;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration), "Configuration cannot be null.");
        }

        public bool SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(toEmail))
                    throw new ArgumentException("Recipient email address cannot be null or empty.", nameof(toEmail));

                // Debugging: Print configuration values
                Console.WriteLine($"SMTP Host: {_configuration["Smtp:Host"]}");
                Console.WriteLine($"SMTP Port: {_configuration["Smtp:Port"]}");
                Console.WriteLine($"SMTP Username: {_configuration["Smtp:Username"]}");
                Console.WriteLine($"SMTP Password: {_configuration["Smtp:Password"]}");
                Console.WriteLine($"SMTP SenderEmail: {_configuration["Smtp:SenderEmail"]}");

                var fromEmail = _configuration["Smtp:SenderEmail"];
                var smtpHost = _configuration["Smtp:Host"];
                var smtpPort = _configuration["Smtp:Port"];
                var smtpUsername = _configuration["Smtp:Username"];
                var smtpPassword = _configuration["Smtp:Password"];

                if (string.IsNullOrWhiteSpace(fromEmail) || string.IsNullOrWhiteSpace(smtpHost) ||
                    string.IsNullOrWhiteSpace(smtpPort) || string.IsNullOrWhiteSpace(smtpUsername) ||
                    string.IsNullOrWhiteSpace(smtpPassword))
                {
                    throw new InvalidOperationException("SMTP settings are not properly configured.");
                }

                using (var smtpClient = new SmtpClient(smtpHost))
                {
                    smtpClient.Port = int.Parse(smtpPort);
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtpClient.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(fromEmail),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(toEmail);
                    smtpClient.Send(mailMessage);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }
}
