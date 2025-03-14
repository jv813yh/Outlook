using System.Net.Mime;
using System.Windows;
using Outlook.Core;
using Outlook.Core.Dialogs;
using Outlook.Core.Interfaces;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace Outlook.Wpf.Core.Dialogs
{

    /// <summary>
    /// Custom dialog service for the WPF application
    /// </summary>
    public class MyDialogService : DialogServiceBase, IMyDialogService
    {
        private readonly IRegionManager _regionManager;
        private readonly IContainerExtension _containerExtension;


        public MyDialogService(IContainerExtension containerExtension,
            IRegionManager regionManager) 
            : base(containerExtension)
        {
            _regionManager = regionManager;
            _containerExtension = containerExtension;
        }

        public void Show(string name)
        {
            var window = _containerExtension.Resolve<RibbonWindow>();

            var newRegionManager = _regionManager.CreateRegionManager();
            RegionManager.SetRegionManager(window, newRegionManager);

            newRegionManager.RequestNavigate(RegionNames.ContentRegion, name);

            window.Owner = Application.Current.MainWindow;
            window.Show();
        }
    }
}
