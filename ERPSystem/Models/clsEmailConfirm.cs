using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace ERPSystem.Models
{
    public class clsEmailConfirm : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var fMail = "ahmed.saeed.dev5@gmail.com";
            var fPassword = ""; // App Password

            var theMsg = new MailMessage();
            theMsg.From = new MailAddress(fMail);
            theMsg.To.Add(email);
            theMsg.Subject = subject;
            theMsg.Body = $"<html><body>{htmlMessage}</body></html>";
            theMsg.IsBodyHtml = true;

            using var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(fMail, fPassword)
            };

            await smtpClient.SendMailAsync(theMsg);

        }
    }
}
