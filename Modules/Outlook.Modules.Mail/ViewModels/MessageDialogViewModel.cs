using Outlook.Business;
using Outlook.Services.Interfaces.MailInterfaces;
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
            set
            {
                SetProperty(ref _currentMailMessage, value);
                CurrentMailMessage.DataSent = System.DateTime.Now;
            }
        }

        private DelegateCommand _sendMessageCommand;
        public DelegateCommand SendMessageCommand =>
            _sendMessageCommand ??= new DelegateCommand(ExecuteSendMessageCommand);

        private DelegateCommand _messageCommand;
        public DelegateCommand MessageCommand =>
            _messageCommand ??= new DelegateCommand(ExecuteMessageCommand);

        private DelegateCommand _newMessageCommand;
        public DelegateCommand NewMessageCommand =>
            _newMessageCommand ??= new DelegateCommand(ExecuteNewMessageCommand);

        private void ExecuteNewMessageCommand()
        {
        }




        /// <summary>
        /// Command to delete selected mail message
        /// </summary>
        private DelegateCommand _deleteMessageCommand;
        public DelegateCommand DeleteMessageCommand =>
            _deleteMessageCommand ?? (_deleteMessageCommand = new DelegateCommand(ExecuteDeleteMessageCommand));
        private void ExecuteDeleteMessageCommand()
        {

        }


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
            _mailService.SentMailMailMessages(CurrentMailMessage);

            IDialogParameters dialogParameters = new DialogParameters();
            dialogParameters.Add(FolderParameters.MessageSent, CurrentMailMessage);

            MessageBox.Show("Message was sent sucessfully", "Information", 
                MessageBoxButton.OK, MessageBoxImage.Information);

            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, dialogParameters));

        }

        public bool CanCloseDialog()
         => true;
        
        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            var messageId = parameters.GetValue<int?>(FolderParameters.MailMessageKey);

            // if no id is passed, then we are creating a new message
            if (!messageId.HasValue)
            {
                CurrentMailMessage = new MailMessage() 
                {
                    // default values
                    From = "test123@microsoft.com",
                };
            }
            else
            {
                CurrentMailMessage = _mailService.GetMessageById(messageId.Value);
            }
        }


        public string Title
        {
            get => "Message Dialog Test";
        }

        public event Action<IDialogResult>? RequestClose;
    }
}
