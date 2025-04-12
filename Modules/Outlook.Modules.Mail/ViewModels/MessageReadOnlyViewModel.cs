using Outlook.Business;
using Outlook.Services.Interfaces.MailInterfaces;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace Outlook.Modules.Mail.ViewModels
{
	public class MessageReadOnlyViewModel : BindableBase, IDialogAware
	{
        private readonly IMailService _mailService;

        private MailMessage _currentMailMessage;

        public event Action<IDialogResult> RequestClose;

        public MailMessage CurrentMailMessage
        {
            get => _currentMailMessage;
            set => SetProperty(ref _currentMailMessage, value);
        }

        public string Title => string.Empty; 

        public MessageReadOnlyViewModel(IMailService mailService)
        {
            _mailService = mailService;
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        /// <summary>
        /// Get data from the dialog during intialization
        /// </summary>
        /// <param name="parameters"></param>
        public void OnDialogOpened(IDialogParameters parameters)
        {
            var messageId = parameters.GetValue<int?>(MailParameters.MailMessageId);

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
    }
}
