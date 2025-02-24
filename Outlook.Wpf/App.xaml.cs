﻿using Infragistics.Windows.OutlookBar;
using Infragistics.Windows.Ribbon;
using Outlook.Modules.Calendar;
using Outlook.Modules.Contacts;
using Outlook.Modules.Mail;
using Outlook.Wpf.Core.Regions;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using System.Windows;
using Outlook.Core.Interfaces;
using Outlook.Core.Commands;
using Outlook.Modules.Mail.Menus;
using Outlook.Modules.Mail.ViewModels;
using Outlook.Wpf.ViewModels;
using Prism.Mvvm;

namespace Outlook.Wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : PrismApplication
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<IApplicationCommands, ApplicationCommands>();

        ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();
    }

    protected override Window CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }


    /// <summary>
    /// Configure all the modules that are used
    /// in the WPF application
    /// for the Prism framework
    /// </summary>
    /// <param name="moduleCatalog"></param>
    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
        moduleCatalog.AddModule<MailModule>();
        moduleCatalog.AddModule<ContactModule>();
        moduleCatalog.AddModule<CalendarModule>();
    }


    /// <summary>
    /// Configure the region adapter mappings
    /// for UI controls that are used in the WPF application
    /// and they need to be registered with the Prism framework to be used for region
    /// </summary>
    /// <param name="regionAdapterMappings"></param>
    protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
    {
        base.ConfigureRegionAdapterMappings(regionAdapterMappings);

        // Customers region adapter mappings
        regionAdapterMappings.RegisterMapping(typeof(XamOutlookBar), Container.Resolve<XamOutlookBarRegionAdapter>());
        regionAdapterMappings.RegisterMapping(typeof(XamRibbon), Container.Resolve<XamRibbonRegionAdapter>());
    }
}

