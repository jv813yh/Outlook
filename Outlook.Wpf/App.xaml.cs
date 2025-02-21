using Prism.Ioc;
using Prism.Unity;
using System.Windows;
using Outlook.Modules.Mail;
using Prism.Modularity;
using Outlook.Modules.Contacts;
using Outlook.Modules.Calendar;

namespace Outlook.Wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : PrismApplication
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        
    }

    protected override Window CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }


    /// <summary>
    /// Configure all the modules that are used in the WPF application
    /// </summary>
    /// <param name="moduleCatalog"></param>
    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
        moduleCatalog.AddModule<MailModule>();
        moduleCatalog.AddModule<ContactModule>();
        moduleCatalog.AddModule<CalendarModule>();
    }
}

