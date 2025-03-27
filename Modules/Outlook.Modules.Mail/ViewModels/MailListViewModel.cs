using Outlook.Core;
using Outlook.Core.Interfaces;
using Outlook.Core.ViewModels;
using Outlook.Modules.Mail.Views;
using Outlook.Services.Interfaces.MailInterfaces;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using MailMessage = Outlook.Business.MailMessage;

namespace Outlook.Modules.Mail.ViewModels
{
	public class MailListViewModel : ViewModelBase
    {
        // Service to work with mail messages
        private readonly IMailService _mailService;

        // Service to work with dialogs
        private readonly IRegionDialogService _dialogService;


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

        private DelegateCommand _messageCommand;
        public DelegateCommand MessageCommand =>
            _messageCommand ?? (_messageCommand = new DelegateCommand(ExecuteMessageCommand));


        private DelegateCommand _newMessageCommand;
        public DelegateCommand NewMessageCommand =>
            _newMessageCommand ?? (_newMessageCommand = new DelegateCommand(ExecuteNewMessageCommand));

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

                });
        }


        public event Action<IDialogResult>? RequestClose;

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

            // Remove from the database
            _mailService.DeleteMessageById(SelectedMailMessage.Id);

            // Remove from the UI
            MailMessages.Remove(SelectedMailMessage);
        }


        // show UI dialog for writing, sending emails
        private void ExecuteMessageCommand()
        {
            if (SelectedMailMessage == null)
            {
                return;
            }

            var parameters = new DialogParameters();
            parameters.Add(FolderParameters.MailMessageKey, SelectedMailMessage.Id);
            // show dialog 
            _dialogService.ShowRegionDialog(RegionNames.ContentRegion, 
                nameof(MessageDialogView), 
                parameters,
                dialogResult =>
                {

                });
        }

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
            var folder = navigationContext.Parameters.GetValue<string>(FolderParameters.FolderKey);

            switch (folder)
            {
                case FolderParameters.Inbox:
                    MailMessages = new ObservableCollection<MailMessage>(_mailService.GetMailMessages());
                    break;
                case FolderParameters.Sent:
                    MailMessages = new ObservableCollection<MailMessage>(_mailService.SentMailMailMessages());
                    break;
                case FolderParameters.Deleted:
                    MailMessages = new ObservableCollection<MailMessage>(_mailService.GetDeletedMessages());
                    break;
            }

            // set the first mail message as selected
            SelectedMailMessage = MailMessages.FirstOrDefault();
        }
    }
}
