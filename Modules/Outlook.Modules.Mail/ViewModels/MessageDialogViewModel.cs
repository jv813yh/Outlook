using Outlook.Business;
using Outlook.Modules.Mail.Models;
using Outlook.Services.Interfaces.MailInterfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
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
            }
        }

        private DelegateCommand _sendMessageCommand;
        public DelegateCommand SendMessageCommand =>
            _sendMessageCommand ??= new DelegateCommand(ExecuteSendMessageCommand);

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
            // Simulate sending a message
            _mailService.SentMailMailMessages(CurrentMailMessage);

            IDialogParameters dialogParameters = new DialogParameters();
            dialogParameters.Add(FolderParameters.MessageSent, CurrentMailMessage);

            MessageBox.Show("Message was sent sucessfully", "Information", 
                MessageBoxButton.OK, MessageBoxImage.Information);

            // Invoke the RequestClose event to close the dialog with the parameters
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, dialogParameters));

        }

        public bool CanCloseDialog()
         => true;
        
        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            string defaultTestEmail = "test123@microsoft.com";

            var messageId = parameters.GetValue<int?>(MailParameters.MailMessageId);
            MessageModes messageMode = parameters.GetValue<MessageModes>(MailParameters.MailMessageMode);

            // 
            ExecuteShowingDialog(messageId, messageMode);
        }


        public string Title
        {
            get => "Message Dialog Test";
        }

        public event Action<IDialogResult>? RequestClose;


        void ExecuteShowingDialog(int? messageId, MessageModes messageModes)
        {

            switch (messageModes)
            {
                case MessageModes.New:
                    InitializeDefaultValues();

                    break;
                case MessageModes.Reply:
                    PrepareMailMessageForReply(messageId);

                    break;
                case MessageModes.ReplyAll:
                    //var toEmails = new ObservableCollection<string>(originalEmail.To);
                    //toEmails.Add(originalEmail.From);
                    //toEmails.AddRange(originalMessage.Cc);

                    break;
                case MessageModes.Forward:
                    break;
                case MessageModes.ReadOnly:
                    break;
            }
        }

        void InitializeDefaultValues()
        {
            // Default values
            string defaultTestEmail = "test123@microsoft.com";

            CurrentMailMessage = new MailMessage()
            {
                // default values
                From = defaultTestEmail,
            };

        }

        void PrepareMailMessageForReply(int? messageId)
        {
            if (CurrentMailMessage == null)
            {
                InitializeDefaultValues();
            }

            if(messageId.HasValue)
            {
                // we need the original message
                var originalMessage = _mailService.GetMessageById(messageId.Value);

                // set the to field
                var toEmails = new ObservableCollection<string>();
                toEmails.Add(originalMessage.From);
                CurrentMailMessage.To = toEmails;

                // append RE to the subject
                CurrentMailMessage.Subject = "RE: " + originalMessage.Subject;

                // TBD
                CurrentMailMessage.Body = originalMessage.Body;
            }
        }
    }
}
