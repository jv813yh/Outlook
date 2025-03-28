
using Outlook.Business;

namespace Outlook.Services.Interfaces.MailInterfaces
{
    public interface IMailService
    {
        IList<MailMessage> GetMailMessages();
        IList<MailMessage> GetSentMailMailMessages();
        IList<MailMessage> GetDeletedMessages();
        MailMessage GetMessageById(int id);
        void SentMailMailMessages(MailMessage currentMailMessage);
        void DeleteMessageById(int id);
    }
}
