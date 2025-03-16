using Outlook.Core;
using Outlook.Core.Dialogs;
using Outlook.Core.Interfaces;
using Prism.Ioc;
using Prism.Regions;
using System.Windows;
using Outlook.Wpf.Core.Dialogs.Controls;
using Prism.Services.Dialogs;

namespace Outlook.Wpf.Core.Dialogs
{

    /// <summary>
    /// Custom dialog service for the WPF application
    /// </summary>
    public class RegionDialogService : DialogServiceBase, IRegionDialogService
    {
        private readonly IRegionManager _regionManager;
        private readonly IContainerExtension _containerExtension;


        public RegionDialogService(IContainerExtension containerExtension,
            IRegionManager regionManager) 
            : base(containerExtension)
        {
            _regionManager = regionManager;
            _containerExtension = containerExtension;
        }

        public void ShowRegionDialog(string name)
        {
            var window = _containerExtension.Resolve<RibbonDialogWindow>();

            var newRegionManager = _regionManager.CreateRegionManager();
            RegionManager.SetRegionManager(window, newRegionManager);

            newRegionManager.RequestNavigate(RegionNames.ContentRegion, name);

            Action<IDialogResult> requestCloseHandler = null;
            requestCloseHandler = (r) =>
            {
                window.Close();
            };

            // RoutedEventHandler is a predefined delegate in WPF
            // that is used to handle routed events (e.g. Loaded, Click).
            RoutedEventHandler loadedHandler = null;
            loadedHandler = (s, e) =>
            {
                window.Loaded -= loadedHandler;
                // The DataContext is a reference to the ViewModel,
                // which handles the data and logic for the View.
                IDialogAware da = window.DataContext as IDialogAware;

                //da.RequestClose += requestCloseHandler;
                if (da != null)
                {
                    da.RequestClose += requestCloseHandler;
                }
            };
            window.Loaded += loadedHandler;

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Show();
        }
    }
}
