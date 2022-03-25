using Asana.Application.Common.Enums;
using Asana.Application.Common.Models;
using System.Threading.Tasks;

namespace Asana.Application.Common.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailMessage emailMessage , EmailType type);

        void SendEmail(EmailMessage emailMessage, EmailType type);
    }
}
