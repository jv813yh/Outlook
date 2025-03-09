using Outlook.Core;
using Outlook.Modules.Mail.Menus;
using Outlook.Modules.Mail.ViewModels;
using Outlook.Modules.Mail.Views;
using Outlook.Services.Interfaces.MailInterfaces;
using Outlook.Services.MailServices;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
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
            // if we want to implicitly register the view model with the view
            ViewModelLocationProvider.Register<MailGroup, MailGroupViewModel>();

            // Register the mail service with the container
            containerRegistry.RegisterSingleton<IMailService, MailProvider>();

            // Register the views with the container for navigation
            containerRegistry.RegisterForNavigation<MailList, MailListViewModel>();

            // Register the dialog with the container
            containerRegistry.RegisterDialog<MessageDialogView, MessageDialogViewModel>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Register the views with the region manager that are used in the Mail module
           // _regionManager.RegisterViewWithRegion(RegionNames.RibbonRegion, typeof(HomeTab));
            _regionManager.RegisterViewWithRegion(RegionNames.OutlookBarRegion, typeof(MailGroup));
        }
    }
}
