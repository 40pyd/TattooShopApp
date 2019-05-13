
using MimeKit;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace _2ScullTattooShop.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            await Task.Factory.StartNew(() =>
            {
                MailMessage _message = new MailMessage("2sculltattoo2000@gmail.com", email);
                _message.Subject = subject;
                _message.Body = message;
                _message.IsBodyHtml = true;
                var client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                NetworkCredential NC = new NetworkCredential("2sculltattoo2000@gmail.com", "2scull04091985");
                client.UseDefaultCredentials = true;
                client.Credentials = NC;
                client.Port = 587;
                client.Send(_message);
            });
        }
    }
}
