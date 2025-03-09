using Outlook.Business;
using Outlook.Core.ViewModels;
using Outlook.Services.Interfaces.MailInterfaces;
using Prism.Commands;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Windows;
using Outlook.Modules.Mail.Views;
using Prism.Services.Dialogs;

namespace Outlook.Modules.Mail.ViewModels
{
	public class MailListViewModel : ViewModelBase
    {
        // Service to work with mail messages
        private readonly IMailService _mailService;

        // Service to work with dialogs
        private readonly IDialogService _dialogService;


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

        private void ExecuteMessageCommand()
        {
            // TODO
            _dialogService.Show(nameof(MessageDialogView));
        }

        public MailListViewModel(IMailService mailService, 
                                 IDialogService dialogService)
        {
            _mailService = mailService;
            _dialogService = dialogService;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
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
