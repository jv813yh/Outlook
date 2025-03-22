using System.Windows;
using Outlook.Core;
using Outlook.Core.Interfaces;
using Outlook.Modules.Mail.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace Outlook.Modules.Mail.ViewModels
{
    /// <summary>
    /// View model for dialog message
    /// </summary>
	public class MessageDialogViewModel : BindableBase, IDialogAware, IRegionManagerAware
    {
        private DelegateCommand _messageCommand;
        public DelegateCommand MessageCommand =>
            _messageCommand ??= new DelegateCommand(ExecuteMessageCommand);

        public IRegionManager RegionManager { get; set; }

        private string _input;
        public string Input
        {
            get => _input;
            set => SetProperty(ref _input, value);
        }

        public MessageDialogViewModel()
        {

        }

        private void ExecuteMessageCommand()
        {
            // Todo: close dialog
            //RequestClose?.Invoke(new DialogResult());
            if (RegionManager != null)
            {
                RegionManager.RequestNavigate(RegionNames.ContentRegion, nameof(MailList));
            }
        }

        public bool CanCloseDialog()
        {
            MessageBoxResult result = MessageBox.Show("Do you really want to close window ?", "MessageDialog Window",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            return result == MessageBoxResult.OK ? true : false;
        }
        
        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        public string Title
        {
            get => "Message Dialog Test";
        }


        public event Action<IDialogResult>? RequestClose;
    }
}
