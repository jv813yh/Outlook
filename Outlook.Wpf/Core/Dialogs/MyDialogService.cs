using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace Outlook.Wpf.Core.Dialogs
{

    /// <summary>
    /// Custom dialog service for the WPF application
    /// </summary>
    public class MyDialogService : DialogServiceBase
    {
        private readonly IRegionManager _regionManager;
        public MyDialogService(IContainerExtension containerExtension,
            IRegionManager regionManager) 
            : base(containerExtension)
        {
            _regionManager = regionManager;
        }

        protected override void InitialDialogWindow(string name, IDialogParameters parametrs)
        {
            (DialogWindow as RibbonWindow).Initialize(name, parametrs);
        }
    }
}
