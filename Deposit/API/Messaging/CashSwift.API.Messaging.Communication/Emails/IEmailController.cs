using System.Threading.Tasks;

namespace CashSwift.API.Messaging.Communication.Emails
{
    public interface IEmailController
    {
        Task<EmailResponse> SendEmailAsync(EmailRequest request);
    }
}
