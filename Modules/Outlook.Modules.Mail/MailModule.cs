using Outlook.Core;
using Outlook.Modules.Mail.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Outlook.Modules.Mail
{
    public class MailModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public MailModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //throw new NotImplementedException();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(ViewA));
        }
    }
}
