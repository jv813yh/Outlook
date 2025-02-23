using Infragistics.Themes;
using Infragistics.Windows.OutlookBar;
using Infragistics.Windows.Ribbon;
using Outlook.Core;
using Outlook.Core.Interfaces;
using Prism.Regions;
using System.Windows;

namespace Outlook.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : XamRibbonWindow
{
    private readonly IRegionManager _regionManager; 
    public MainWindow(IRegionManager regionManager)
    {
        InitializeComponent();

        _regionManager = regionManager;

        // Set the application theme to Office2013
        ThemeManager.ApplicationTheme = new Office2013Theme();
    }

    private void XamOutlookBar_OnSelectedGroupChanged(object sender, RoutedEventArgs e)
    {
        var group = (sender as XamOutlookBar).SelectedGroup as IOutlookBarGroup;
        if (group != null)
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegion, group.DefaultNavigationPath);
        }
    }
}    