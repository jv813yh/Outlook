
using Outlook.Business;

namespace Outlook.Services.Interfaces.MailInterfaces
{
    public interface IMailService
    {
        IList<MailMessage> GetMailMessages();
    }
}
