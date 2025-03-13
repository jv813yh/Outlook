using System.Collections.ObjectModel;
using System.Globalization;
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
                return string.Join(", ", _emails);
            }

            return string.Empty;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // TODO: Implement this method
            return value;
        }
    }
}
