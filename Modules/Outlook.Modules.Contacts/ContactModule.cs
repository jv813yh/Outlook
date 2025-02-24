using Outlook.Core;
using Outlook.Modules.Contacts.Menus;
using Outlook.Modules.Contacts.ViewModels;
using Outlook.Modules.Contacts.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Outlook.Modules.Contacts
{
    public class ContactModule : IModule
    {
        private readonly IRegionManager _regionManager;
        public ContactModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
           containerRegistry.RegisterForNavigation<ViewA, ViewAViewModel>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.OutlookBarRegion, typeof(ContactsGroup));
        }
    }
}
