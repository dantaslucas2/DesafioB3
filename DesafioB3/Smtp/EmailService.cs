using DesafioB3.Models;
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
        private static bool wasConfigured = false;
        private static string smtpHost;
        private static int smtpPort;
        private static string smtpUsername;
        private static string smtpAppPassword;

        private static string fromAddress;
        private static string toAddress;

        private static string fileName = "appsetings.json";

        public static void Config()
        {
            try
            {
                var temp = AppDomain.CurrentDomain.BaseDirectory;

                var current = Directory.GetCurrentDirectory();
                var dicFilepath = Directory.GetParent(Directory.GetParent(Directory.GetParent(current).FullName).FullName);
                string json = File.ReadAllText(Path.Combine(dicFilepath.FullName, fileName));
                var JsonObject = JsonConvert.DeserializeObject<ModelConfig>(json);

                fromAddress = JsonObject.SenderEmail;
                smtpUsername = fromAddress;
                smtpAppPassword = JsonObject.Password;
                toAddress = JsonObject.RecipientEmail;
                smtpHost = JsonObject.SmtpHost;
                smtpPort = JsonObject.SmtpPort;
                wasConfigured = true;
#if DEBUG
                Console.WriteLine("Email successfully configured");
#endif
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error loading email settings: {e}");
                throw new NotImplementedException();
            }
        }

        public static bool SendEmail(string asset, bool IsBuyer, decimal value)
        {
            if (!wasConfigured) Config();
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

                MailMessage mail = new MailMessage(fromAddress, toAddress, subject, body);
                mail.IsBodyHtml = false;

                SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpAppPassword);
                smtpClient.EnableSsl = true;

                smtpClient.Send(mail);
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
