using Outlook.Business;
using Outlook.Services.Interfaces.MailInterfaces;
using System.Collections.ObjectModel;

namespace Outlook.Services.MailServices
{
    public class MailProvider : IMailService
    {
        static List<MailMessage> _testMailMessages = new List<MailMessage>()
        {
            new MailMessage()
            {
                Id = 1,
                From = "jozefko@gmail.com",
                Subject = "Test message",
                To = new ObservableCollection<string>()
                {
                    "jane@doey.com",
                    "peter@doey.com"

                }
            },

        };

        public IList<MailMessage> GetMailMessages()
        {
            throw new NotImplementedException();
        }
    }
}
