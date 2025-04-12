using Outlook.Core;
using Outlook.Core.Interfaces;
using Outlook.Core.ViewModels;
using Outlook.Modules.Mail.Models;
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


        private DelegateCommand<string> _selectMessageCommand;
        public DelegateCommand<string> SelectMessageCommand =>
            _selectMessageCommand ?? (_selectMessageCommand = new DelegateCommand<string>(ExecuteSelectMessageCommand));

        private void ExecuteSelectMessageCommand(string parameter)
        {
            if(SelectedMailMessage == null)
                return;

            //  Default values
            string viewName = "MessageDialogView";
            int? mailMessageId = null;
            MessageModes messageModes = MessageModes.New;

            // set the values according to the parameter
            SetValues(parameter, ref viewName, ref messageModes, ref mailMessageId);


            // Create parameters for the dialog
            DialogParameters parameters = new DialogParameters();
            parameters.Add(MailParameters.MailMessageId, mailMessageId);
            // add parameters to the dialog
            parameters.Add(MailParameters.MailMessageMode, messageModes);

            // show dialog
            OpenDialogWindow(viewName, parameters);
        }

        private void OpenDialogWindow(string viewName, DialogParameters parameters)
        {
            var messageId = parameters.GetValue<int?>(MailParameters.MailMessageId);

            if (messageId.HasValue)
            {
                _dialogService.ShowRegionDialog(RegionNames.ContentRegion,
                     viewName,
                     parameters,
                     dialogResult =>
                     {
                         if (dialogResult.Result == ButtonResult.OK)
                         {
                             // do something
                         }
                     });
            }
            else
            {
                _dialogService.ShowRegionDialog(RegionNames.ContentRegion,
                   viewName,
                   parameters,
                   dialogResult =>
                   {
                       SetCorretMessageFromDialog(dialogResult);
                   });

            }
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

        private DelegateCommand _replyMessageCommand;
        public DelegateCommand ReplyMessageCommand =>
            _replyMessageCommand ?? (_replyMessageCommand = new DelegateCommand(ExecuteReplyMessageCommand));

        private void ExecuteReplyMessageCommand()
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="viewName"></param>
        /// <param name="messageModes"></param>
        void SetValues(string parameter,
             ref string viewName,
             ref MessageModes messageModes,
             ref int? mailMessageId)
        {
            mailMessageId = SelectedMailMessage.Id;

            if (parameter == nameof(MessageModes.ReadOnly))
            {
                viewName = "MessageReadOnlyView";
                messageModes = MessageModes.ReadOnly;
            }
            else if (parameter == nameof(MessageModes.Reply))
            {
                messageModes = MessageModes.Reply;
            }
            else if (parameter == nameof(MessageModes.ReplyAll))
            {
                messageModes = MessageModes.ReplyAll;
            }
            else if (parameter == nameof(MessageModes.Forward))
            {
                messageModes = MessageModes.Forward;
            }
            else if (parameter == nameof(MessageModes.New))
            {
                messageModes = MessageModes.New;
                mailMessageId = null;
            }
        }
    }
}
