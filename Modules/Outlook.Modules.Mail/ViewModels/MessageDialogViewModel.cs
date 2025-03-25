using Outlook.Business;
using Outlook.Services.Interfaces.MailInterfaces;
using Outlook.Services.MailServices;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Windows;

namespace Outlook.Modules.Mail.ViewModels
{
    /// <summary>
    /// View model for dialog message
    /// </summary>
	public class MessageDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IMailService _mailService;

        private MailMessage? _currentMailMessage;
        public MailMessage? CurrentMailMessage
        {
            get => _currentMailMessage;
            set => SetProperty(ref _currentMailMessage, value);
        }

        private DelegateCommand _sendMessageCommand;
        public DelegateCommand SendMessageCommand =>
            _sendMessageCommand ??= new DelegateCommand(ExecuteSendMessageCommand);

        private DelegateCommand _messageCommand;
        public DelegateCommand MessageCommand =>
            _messageCommand ??= new DelegateCommand(ExecuteMessageCommand);


        private void ExecuteMessageCommand()
        {
            
        }

        private string _input;
        public string Input
        {
            get => _input;
            set => SetProperty(ref _input, value);
        }

        public MessageDialogViewModel(IMailService mailService)
        {
            _mailService = mailService;
        }

        private void ExecuteSendMessageCommand()
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
            var messageId = parameters.GetValue<int>("id");

            try
            {
                CurrentMailMessage = _mailService.GetMessageById(messageId);
            }
            catch (Exception)
            {
                MessageBox.Show("Error loading message");
            }
        }

        public string Title
        {
            get => "Message Dialog Test";
        }


        public event Action<IDialogResult>? RequestClose;
    }
}
