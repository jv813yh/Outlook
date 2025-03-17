using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace Outlook.Modules.Mail.ViewModels
{
    /// <summary>
    /// View model for dialog message
    /// </summary>
	public class MessageDialogViewModel : BindableBase, IDialogAware
    {
        private DelegateCommand _messageCommand;
        public DelegateCommand MessageCommand =>
            _messageCommand ??= new DelegateCommand(ExecuteMessageCommand);

        private string _input;
        public string Input
        {
            get => _input;
            set => SetProperty(ref _input, value);
        }

        public MessageDialogViewModel()
        {

        }

        private void ExecuteMessageCommand()
        {
            // Todo: close dialog
            RequestClose?.Invoke(new DialogResult());
        }

        public bool CanCloseDialog()
         => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        public string Title
        {
            get => "Mail Message Test";
        }

        public event Action<IDialogResult>? RequestClose;
    }
}
