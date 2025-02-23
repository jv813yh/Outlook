﻿using Outlook.Core;
using Outlook.Modules.Mail.Menus;
using Outlook.Modules.Mail.ViewModels;
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
            ViewModelLocationProvider.Register<MailGroup, MailGroupViewModel>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Register the views with the region manager that are used in the Mail module
            _regionManager.RegisterViewWithRegion(RegionNames.RibbonRegion, typeof(HomeTab));
            _regionManager.RegisterViewWithRegion(RegionNames.OutlookBarRegion, typeof(MailGroup));
        }
    }
}
