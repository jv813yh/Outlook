using Outlook.Core;
using Outlook.Core.Interfaces;
using Outlook.Core.ViewModels;
using Outlook.Modules.Mail.Views;
using Outlook.Services.Interfaces.MailInterfaces;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Windows;
using MailMessage = Outlook.Business.MailMessage;

namespace Outlook.Modules.Mail.ViewModels
{
	public class MailListViewModel : ViewModelBase
    {
        // Service to work with mail messages
        private readonly IMailService _mailService;

        // Service to work with dialogs
        private readonly IRegionDialogService _dialogService;

        private string _currentFolder = FolderParameters.Inbox;

        private string _title = "Default";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        // selected mail message from UI
        private MailMessage _selectedMailMessage;
        public MailMessage SelectedMailMessage
        {
            get { return _selectedMailMessage; }
            set { SetProperty(ref _selectedMailMessage, value); }
        }

        // list of mail messages
        private ObservableCollection<MailMessage> _mailMessages;
        public ObservableCollection<MailMessage> MailMessages
        {
            get { return _mailMessages; }
            set { SetProperty(ref _mailMessages, value); }
        }

        /// <summary>
        /// Command to show UI dialog for writing, sending emails
        /// </summary>

        private DelegateCommand _replyAllMessageCommand;
        public DelegateCommand ReplyAllMessageCommand =>
            _replyAllMessageCommand ?? (_replyAllMessageCommand = new DelegateCommand(ExecuteReplyAllMessageCommand));

        private void ExecuteReplyAllMessageCommand()
        {
        }

        private DelegateCommand _forwardMessageCommand;
        public DelegateCommand ForwardMessageCommand =>
            _forwardMessageCommand ?? (_forwardMessageCommand = new DelegateCommand(ExecuteForwardMessageCommand));

        private void ExecuteForwardMessageCommand()
        {
        }

        private DelegateCommand _newMessageCommand;
        public DelegateCommand NewMessageCommand =>
            _newMessageCommand ?? (_newMessageCommand = new DelegateCommand(ExecuteNewMessageCommand));


        private DelegateCommand _replyMessageCommand;
        public DelegateCommand ReplyMessageCommand =>
            _replyMessageCommand ?? (_replyMessageCommand = new DelegateCommand(ExecuteReplyMessageCommand));

        private void ExecuteReplyMessageCommand()
        {
        }

        private void ExecuteNewMessageCommand()
        {
            var parameters = new DialogParameters();
            parameters.Add(FolderParameters.MailMessageKey, null);
            // show dialog 
            _dialogService.ShowRegionDialog(RegionNames.ContentRegion,
                nameof(MessageDialogView),
                parameters,
                dialogResult =>
                {
                    SetCorretMessageFromDialog(dialogResult);
                });
        }

        /// <summary>
        /// Command to delete selected mail message
        /// </summary>
        private DelegateCommand _deleteMessageCommand;
        public DelegateCommand DeleteMessageCommand =>
            _deleteMessageCommand ?? (_deleteMessageCommand = new DelegateCommand(ExecuteDeleteMessageCommand));
        private void ExecuteDeleteMessageCommand()
        {
            if(SelectedMailMessage == null)
                return;

            var messageBoxResult = MessageBox.Show("Are you sure you want to delete this message?", 
                                                        "Delete message", MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.No)
                return;

            // Remove from the database
            _mailService.DeleteMessageById(SelectedMailMessage.Id);

            // Remove from the UI
            MailMessages.Remove(SelectedMailMessage);
        }


        public event Action<IDialogResult>? RequestClose;

        public MailListViewModel(IMailService mailService,
            IRegionDialogService dialogService)
        {
            _mailService = mailService;
            _dialogService = dialogService;

            MailMessages = new ObservableCollection<MailMessage>();
        }

        /// <summary>
        /// Load mail messages according to the folder during navigation
        /// </summary>
        /// <param name="navigationContext"></param>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            // according the key we can get value
            _currentFolder = navigationContext.Parameters.GetValue<string>(FolderParameters.FolderKey);

            //
            LoadMessages(_currentFolder);

            // set the first mail message as selected
            SelectedMailMessage = MailMessages.FirstOrDefault();
        }


        void LoadMessages(string folder)
        {
            switch (folder)
            {
                case FolderParameters.Inbox:
                    MailMessages = new ObservableCollection<MailMessage>(_mailService.GetMailMessages());
                    break;
                case FolderParameters.Sent:
                    MailMessages = new ObservableCollection<MailMessage>(_mailService.GetSentMailMailMessages());
                    break;
                case FolderParameters.Deleted:
                    MailMessages = new ObservableCollection<MailMessage>(_mailService.GetDeletedMessages());
                    break;
            }
        }

        /// <summary>
        /// Set correct message from dialog
        /// </summary>
        /// <param name="dialogResult"></param>
        void SetCorretMessageFromDialog(IDialogResult dialogResult)
        {
            if (dialogResult.Parameters.TryGetValue<MailMessage>(FolderParameters.MessageSent, out var message))
            {
                // add to sent folder
                MailMessages.Add(message);
            }
        }
    }
}
