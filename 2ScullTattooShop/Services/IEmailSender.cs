using System.Threading.Tasks;

namespace _2ScullTattooShop.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
