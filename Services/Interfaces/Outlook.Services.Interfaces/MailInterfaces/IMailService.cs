
using Outlook.Business;

namespace Outlook.Services.Interfaces.MailInterfaces
{
    public interface IMailService
    {
        IList<MailMessage> GetMailMessages();

        IList<MailMessage> SentMailMailMessages();

        IList<MailMessage> GetDeletedMessages();

        MailMessage GetMessageById(int id);
        void DeleteMessageById(int id);
    }
}
