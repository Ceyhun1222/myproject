using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ObstacleManagementSystem.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class MessageSender : IEmailSender, ISmsSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress("FromUserName", "Title")
                };
                mail.To.Add(new MailAddress(email));

                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com",587))
                {
                    smtp.Credentials = new NetworkCredential("UserName", "password");
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                //do something here
            }

            //SmtpClient client = new SmtpClient("smtp.gmail.com");
            //client.UseDefaultCredentials = false;
            //client.Credentials = new NetworkCredential("abuzar.hasanov", "password");

            //MailMessage mailMessage = new MailMessage();
            //mailMessage.From = new MailAddress("whoever@me.com");
            //mailMessage.To.Add(email);
            //mailMessage.Body = message;
            //mailMessage.Subject = subject;
            //client.Send(mailMessage);
            //var t = Task.CompletedTask;
            //return;
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
