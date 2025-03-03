using System.Windows;
using System.Windows.Input;
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

        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Title = navigationContext.Parameters.GetValue<string>("id");
        }
    }
}
