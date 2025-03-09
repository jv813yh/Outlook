using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace Outlook.Modules.Mail.ViewModels
{
    /// <summary>
    /// View model for dialog message
    /// </summary>
	public class MessageDialogViewModel : BindableBase, IDialogAware
	{
        public MessageDialogViewModel()
        {

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
            get => "Mail Message";
        }

        public event Action<IDialogResult>? RequestClose;
    }
}
