using Outlook.Business;
using Outlook.Services.Interfaces.MailInterfaces;
using System.Collections.ObjectModel;

namespace Outlook.Services.MailServices
{
    public class MailProvider : IMailService
    {


#region Test Data 

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
                },
                Body = "Random String Generator is easy to use tool to generate unique String based on given parameters.",
                DataSent = DateTime.Now
            },
            new MailMessage()
            {
                Id = 2,
                From = "peterko@gmail.com",
                Subject = "Test message 2",
                To = new ObservableCollection<string>()
                {
                    "jonas@doey.com",
                    "jacob@doey.com"
                },
                Body = "You can use scala.util.Random to generate random characters, eg using nextPrintableChar. " +
                       "Combine it with an iterator, to generate a sequence of them and filter",
                DataSent = DateTime.Now.AddDays(-1)
            },
            new MailMessage()
            {
                Id = 3,
                From = "adamko@gmail.com",
                Subject = "Test message 3",
                To = new ObservableCollection<string>()
                {
                    "sebastian@doey.com",
                    "joerg@doey.com"
                },
                Body = "Hello, i was trying to generate a random string. " +
                       "Looking for something similar on google, i find this one way: Random.seed!",
                DataSent = DateTime.Now.AddDays(-2)
            },
            new MailMessage()
            {
                Id = 4,
                From = "davidko@gmail.com",
                Subject = "Test message 4",
                To = new ObservableCollection<string>()
                {
                    "tina@doey.com",
                    "sabrina@doey.com"
                },
                Body = "Generates a random string of given length from characters specified in second argument. " +
                       "Supports intervals, such as 0-9 or A-Z .",
                DataSent = DateTime.Now.AddDays(-3)
            },
            new MailMessage()
            {
                Id = 5,
                From = "karstenko@gmail.com",
                Subject = "Test message 5",
                To = new ObservableCollection<string>()
                {
                    "roland@doey.com",
                    "vladimir@doey.com"
                },
                Body = "Number Picker Wheel is a specialized random number generator, " +
                       "rng tool which picks a random number differently by spinning a wheel. Free and easy to use.",
                DataSent = DateTime.Now.AddDays(-4)
            },

        };

        private static List<MailMessage> _deletedMessages = new List<MailMessage>();

        private static List<MailMessage> _sentMessages = new List<MailMessage>();

#endregion

#region IMailService implementation


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<MailMessage> GetMailMessages()
            => _testMailMessages;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<MailMessage> SentMailMailMessages()
        {
            return _sentMessages;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<MailMessage> GetDeletedMessages()
        {
            return _deletedMessages;
        }

#endregion

    }
}
