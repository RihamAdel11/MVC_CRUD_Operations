using System.Threading.Tasks;

namespace Demo.PLL.Services.EmailSender
{
    public interface IEmailSender
    {
        Task SendAsync(string from, string recipients, string subject, string body);
    }
}
