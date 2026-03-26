using DesafioB3.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace DesafioB3.Smtp
{
    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public bool SendEmail(string asset, bool IsBuyer, decimal value)
        {
            try
            {
                string subject = EmailMenssage.defaultSubject;
                subject = subject.Replace("[Asset Name]", asset);
                string body;

                if (IsBuyer) body = EmailMenssage.defaultMenssageemailBuy;
                else body = EmailMenssage.defaultMenssageemailSell;

                body = body.Replace("[Asset Name]", asset);
                body = body.Replace("[User's Name]", "User's Name");
                body = body.Replace("[Your Name or Company Name]", "Your Name or Company Name");
                body = body.Replace("[Contact Information]", "Contact Information");
                body = body.Replace("[Your Position]", "Your Position");

                MailMessage mail = new MailMessage(_settings.SenderEmail, _settings.RecipientEmail, subject, body);
                mail.IsBodyHtml = false;

                var smtp = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort)
                {
                    Credentials = new NetworkCredential(
                        _settings.SenderEmail,
                        _settings.Password),
                    EnableSsl = true
                };
                smtp.Send(mail);
#if DEBUG
                Console.WriteLine("Email successfully sent");
#endif
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when sending email: {ex.Message}");
                return false;
            }
        }
    }
}
