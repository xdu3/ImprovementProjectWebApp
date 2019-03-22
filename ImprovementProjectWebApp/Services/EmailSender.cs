using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {

                SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 587)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("aesrev@outlook.com", "Dx1522cctv"),
                    EnableSsl = true

            };

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("aesrev@outlook.com")
                };

                mailMessage.To.Add(email);
                mailMessage.Body = message;
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = subject;
                client.Send(mailMessage);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
