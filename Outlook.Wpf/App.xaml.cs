using Infragistics.Windows.OutlookBar;
using Infragistics.Windows.Ribbon;
using Outlook.Core.Commands;
using Outlook.Core.Interfaces;
using Outlook.Modules.Calendar;
using Outlook.Modules.Contacts;
using Outlook.Modules.Mail;
using Outlook.Wpf.Core.Dialogs;
using Outlook.Wpf.Core.Regions;
using Outlook.Wpf.ViewModels;
using Outlook.Wpf.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Prism.Unity;
using System.Windows;

namespace Outlook.Wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : PrismApplication
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        // Register IApplicationCommands as a singleton for ApplicationCommands
        containerRegistry.RegisterSingleton<IApplicationCommands, ApplicationCommands>();

        // All dialogs opened via IDialogService will automatically
        // use RibbonWindow as the base window
        containerRegistry.RegisterDialogWindow<RibbonWindow>();

        // containerRegistry.RegisterDialogWindow<CustomDialogWindow>("CustomDialog");
        // _dialogService.ShowDialog("MyDialogView", new DialogParameters(), result => { }, "CustomDialog");

        // I use custom dialog service for the WPF application isnteaf of the default Prism dialog service
        containerRegistry.RegisterSingleton<IDialogService, MyDialogService>();

        ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();
    }

    /// <summary>
    /// Create the shell window for the WPF application
    /// </summary>
    /// <returns></returns>
    protected override Window CreateShell()
     => Container.Resolve<MainWindow>();


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

    /// <summary>
    /// Register custom region behaviors
    /// </summary>
    /// <param name="regionBehaviors"></param>
    protected override void ConfigureDefaultRegionBehaviors(IRegionBehaviorFactory regionBehaviors)
    {
        base.ConfigureDefaultRegionBehaviors(regionBehaviors);
        regionBehaviors.AddIfMissing(DependentViewRegionBehavior.BehaviorKey, typeof(DependentViewRegionBehavior));
    }
}

