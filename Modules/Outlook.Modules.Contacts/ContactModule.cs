using Outlook.Core;
using Outlook.Modules.Contacts.Menus;
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
           
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.OutlookBarRegion, typeof(ContactsGroup));
        }
    }
}
