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
                Body = _rtfBodyRandomTest,
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
                Body = _rtfBodyWeatherTest,
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
                Body = _rtfBodyCookingTest,
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
                Body = _rtfBodySportsUpdateTest,
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
                Body = _rtfBodyItNewsTest,
                DataSent = DateTime.Now.AddDays(-4)
            },

        };


        const string _rtfBodyRandomTest = "{\\rtf1\\ansi\\deff0\n{\\fonttbl{\\f0 Arial;}}\n{\\colortbl;\\red0\\green0\\blue0;\\red255\\green0\\blue0;}\n\\paperw11906\\" +
                                        "paperh16838\\margl1440\\margr1440\\margt1440\\margb1440\n\n\\pard\\qc\\b\\f0\\fs28 Sample Email Subject\\par\n\\b0\\fs20\n\\" +
                                        "pard\\sa200\nFrom: sender@example.com\\par\nTo: receiver@example.com\\par\nDate: March 4, 2025\\par\nSubject: Sample Email for " +
                                        "Testing\\par\n\\par\nHello,\\par\n\\par\nThis is a sample email in \\b RTF \\b0 format for testing purposes. \\par\nYou can use " +
                                        "this text to verify that your application properly displays and handles rich text content.\\par\n\\par\nBest regards,\\par\nSender Name\\par\n}";

        const string _rtfBodyWeatherTest = "{\\rtf1\\ansi\\deff0\r\n{\\fonttbl{\\f0 Arial;}}\r\n{\\colortbl;\\red0\\green0\\blue0;\\red0\\green0\\blue255;}\r\n\\paperw11906\\" +
                                                   "paperh16838\\margl1440\\margr1440\\margt1440\\margb1440\r\n\\pard\\qc\\b\\f0\\fs28 Weather Update: Sunny Days Ahead\\par\r\n\\b0\\fs20" +
                                                   "\r\n\\pard\\sa200\r\nFrom: weatherdesk@example.com\\par\r\nTo: subscriber@example.com\\par\r\nDate: March 4, 2025\\par\r\nSubject: " +
                                                   "Weekly Weather Forecast\\par\r\n\\par\r\nHello,\\par\r\n\\par\r\nThe forecast for this week looks bright and sunny! Expect clear skies," +
                                                   " warm temperatures, and a gentle breeze throughout the week. Enjoy the pleasant weather and plan some outdoor activities.\\par\r\n\\par" +
                                                   "\r\nBest regards,\\par\r\nWeather Desk\\par\r\n}\r\n";

        const string _rtfBodyCookingTest = "{\\rtf1\\ansi\\deff0\r\n{\\fonttbl{\\f0 Arial;}}\r\n{\\colortbl;\\red0\\green0\\blue0;\\red255\\green0\\blue0;}\r\n\\paperw11906\\" +
                                                   "paperh16838\\margl1440\\margr1440\\margt1440\\margb1440\r\n\\pard\\qc\\b\\f0\\fs28 New Recipe: Delicious Pasta\\par\r\n\\b0\\fs20\r\n\\" +
                                                   "pard\\sa200\r\nFrom: chef@example.com\\par\r\nTo: foodie@example.com\\par\r\nDate: March 4, 2025\\par\r\nSubject: Recipe for a Delicious " +
                                                   "Pasta Dish\\par\r\n\\par\r\nHello,\\par\r\n\\par\r\nToday, I'm excited to share my latest recipe for a delightful pasta dish. Fresh tomatoes," +
                                                   " basil, and garlic are combined to create a rich, flavorful sauce that pairs perfectly with your favorite pasta. Don't forget a sprinkle of " +
                                                   "parmesan cheese for that extra touch of goodness!\\par\r\n\\par\r\nHappy cooking,\\par\r\nChef Gourmet\\par\r\n}\r\n";

        const string _rtfBodySportsUpdateTest = "{\\rtf1\\ansi\\deff0\r\n{\\fonttbl{\\f0 Arial;}}\r\n{\\colortbl;\\red0\\green0\\blue0;\\red0\\green128\\blue0;}\r\n\\paperw11906\\paperh16838" +
                                                "\\margl1440\\margr1440\\margt1440\\margb1440\r\n\\pard\\qc\\b\\f0\\fs28 Sports Update: Local Team Triumphs\\par\r\n\\b0\\fs20\r\n\\pard\\sa200" +
                                                "\r\nFrom: sportsnews@example.com\\par\r\nTo: fan@example.com\\par\r\nDate: March 4, 2025\\par\r\nSubject: Exciting Victory in Last Night's Game" +
                                                "\\par\r\n\\par\r\nHello,\\par\r\n\\par\r\nGreat news from the sports arena! Our local team secured an impressive victory last night in a " +
                                                "thrilling match. The players demonstrated exceptional teamwork and skill, thrilling fans across the city. Stay tuned for more detailed analysis " +
                                                "and upcoming game schedules.\\par\r\n\\par\r\nGo Team!\\par\r\nSports News Team\\par\r\n}\r\n";

        const string _rtfBodyItNewsTest = "{\\rtf1\\ansi\\deff0\r\n{\\fonttbl{\\f0 Arial;}}\r\n{\\colortbl;\\red0\\green0\\blue0;\\red128\\green0\\blue128;}\r\n\\paperw11906\\paperh16838" +
                                          "\\margl1440\\margr1440\\margt1440\\margb1440\r\n\\pard\\qc\\b\\f0\\fs28 IT News: Innovations in Tech\\par\r\n\\b0\\fs20\r\n\\pard\\sa200\r\nFrom:" +
                                          " techupdates@example.com\\par\r\nTo: itprofessional@example.com\\par\r\nDate: March 4, 2025\\par\r\nSubject: Breaking Tech Innovations and Trends" +
                                          "\\par\r\n\\par\r\nHello,\\par\r\n\\par\r\nIn today's IT news, several groundbreaking innovations have been announced by leading tech companies. " +
                                          "These advancements promise to revolutionize cloud computing, artificial intelligence, and cybersecurity. Stay tuned for in-depth articles and " +
                                          "expert insights on these exciting developments.\\par\r\n\\par\r\nBest regards,\\par\r\nTech Updates Team\\par\r\n}\r\n";
        #endregion

        #region IMailService implementation

        private static List<MailMessage> _deletedMessages = new List<MailMessage>();

        private static List<MailMessage> _sentMessages = new List<MailMessage>();

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MailMessage GetMessageById(int id) 
        {
            var allMessages = new List<MailMessage>();
            allMessages.AddRange(_testMailMessages);
            allMessages.AddRange(_sentMessages);
            allMessages.AddRange(_deletedMessages);

            var returnMessage = allMessages.FirstOrDefault(m => m.Id == id);

            return returnMessage != null ? returnMessage : new MailMessage() { Subject = "Not Found Mail"};
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void DeleteMessageById(int id)
        {
            var message = _deletedMessages.FirstOrDefault(m => m.Id == id);
            if (message != null)
            {
                _deletedMessages.Remove(message);
                return;
            }

            message = _testMailMessages.FirstOrDefault(m => m.Id == id);
            if (message != null)
            {
                _testMailMessages.Remove(message);
                _deletedMessages.Add(message);
            }

            message = _sentMessages.FirstOrDefault(m => m.Id == id);
            if (message != null)
            {
                _sentMessages.Remove(message);
                _deletedMessages.Add(message);
            }
        }


        #endregion

    }
}
