using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Outlook.Business;
using Outlook.Core.ViewModels;
using Prism.Commands;
using Prism.Regions;

namespace Outlook.Modules.Mail.ViewModels
{
	public class MailListViewModel : ViewModelBase
    {
        private string _title = "Default";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        // list of mail messages
        private ObservableCollection<MailMessage> _mailMessages;
        public ObservableCollection<MailMessage> MailMessages
        {
            get { return _mailMessages; }
            set { SetProperty(ref _mailMessages, value); }
        }

        private DelegateCommand _testCommand;
        public DelegateCommand TestCommand =>
            _testCommand ?? (_testCommand = new DelegateCommand(ExecuteTestCommand));

        private void ExecuteTestCommand()
        {
            // TODO
            MessageBox.Show("Test Command", "Test",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public MailListViewModel()
        {
            _mailMessages = new ObservableCollection<MailMessage>();
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Title = navigationContext.Parameters.GetValue<string>("id");
        }
    }
}
