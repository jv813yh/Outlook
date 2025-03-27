using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Outlook.Modules.Mail.Converters
{
    public class MailAddressConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            ObservableCollection<string> _emails = value as ObservableCollection<string>;

            if (_emails != null)
            {
                return string.Join(",", _emails);
            }

            return string.Empty;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var emails = value as string;

            if (emails != null)
            {
                var emailsArray = emails.Split(',');
                emailsArray.ToList().ForEach(x => x.Trim());

                return new ObservableCollection<string>(emailsArray);
            }

            return string.Empty;
        }
    }
}
