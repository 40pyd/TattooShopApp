using BLL.Models;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace _2ScullTattooShop.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Confirm your email",
                $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
        }

        public static Task SendEmailOrderConfirmationAsync(this IEmailSender emailSender,string email,OrderDTO order)
        {
            string text = $"<h1>Dear {order.CustomerName}!</h1><h2>Your order is proceded</h2>" +
                "<h4>Order information:</h4>" +
                $"<p><strong>Created on: </strong>{DateTimeOffset.Now}</p>" +
                $"<p><strong>and will be delivered to: </strong>{order.AddressLine} in two working days</p>" +
                $"<p><strong>Total value: </strong>{order.TotalValue.ToString()} $</p><br/><h3>Contact us on</h3>" +
                $"<p>14 Podval<br> Podvalna str<br> Brovary dist<br> Kyiv<br><strong>Ukraine</strong></p><br>" +
                $"<p>This number is toll free if calling from Ukraine otherwise we advise you to use the electronic form of communication.</p><p>"+
                $"<strong>+38 (066)5555555</strong></p><h3>Electronic support</h3><p>Please feel free to write an email to us.</p>"+
                $"<ul><li><strong><a> 40pyd @gmail.com</a></strong></li><li><strong><a> saryk@ukr.net</a></strong></li></ul>"+
                $"<h4>Best regards, your 2ScullTattooStudioShop</h4>";
            return emailSender.SendEmailAsync(email, "Your 2ScullTattooShop order",text);
        }
    }
}
