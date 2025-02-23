using Prism.Mvvm;

namespace Outlook.Wpf.ViewModels
{
    public class MessageViewModel :BindableBase
    {
        private bool _isActive = false;
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        private string _message = string.Empty;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }
    }
}
