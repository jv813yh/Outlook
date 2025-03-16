using Outlook.Business;
using Outlook.Core.Interfaces;
using Outlook.Core.ViewModels;
using Outlook.Modules.Mail.Views;
using Outlook.Services.Interfaces.MailInterfaces;
using Prism.Commands;
using Prism.Regions;
using System.Collections.ObjectModel;

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

        private DelegateCommand _messageCommand;
        public DelegateCommand MessageCommand =>
            _messageCommand ?? (_messageCommand = new DelegateCommand(ExecuteMessageCommand));

        // show UI dialog for writing, sending emails
        private void ExecuteMessageCommand()
        {
            // TODO
            _dialogService.ShowRegionDialog(nameof(MessageDialogView));
        }

        public MailListViewModel(IMailService mailService,
            IRegionDialogService dialogService)
        {
            _mailService = mailService;
            _dialogService = dialogService;
        }

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
        }
    }
}
