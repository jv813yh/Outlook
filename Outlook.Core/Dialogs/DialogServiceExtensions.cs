using Outlook.Core.Dialogs;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Windows;
using Outlook.Core.Interfaces;

namespace Outlook.Core.Dialogs
{
    public static class DialogServiceExtensions
    {
        public static void ShowRibbonWindow(this IDialogService dialogService, string name)
        {
            if (dialogService is IDialogService myDialogService)
            {
                var window = new RibbonWindow();

                var newRegionManager = new RegionManager();
                RegionManager.SetRegionManager(window, newRegionManager);

                newRegionManager.RequestNavigate(RegionNames.ContentRegion, name);

                window.Owner = Application.Current.MainWindow;
                window.Show();
            }
        }
    }
}
