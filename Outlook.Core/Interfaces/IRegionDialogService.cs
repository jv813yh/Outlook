using Prism.Services.Dialogs;

namespace Outlook.Core.Interfaces
{
    public interface IRegionDialogService
    {
        void ShowRegionDialog(string regionName, 
            string viewName, 
            IDialogParameters dialogParameters,
            Action<IDialogResult> callback);
    }
}
