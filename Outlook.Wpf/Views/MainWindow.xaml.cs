using Infragistics.Themes;
using Infragistics.Windows.OutlookBar;
using Infragistics.Windows.Ribbon;
using Outlook.Core.Interfaces;
using System.Windows;

namespace Outlook.Wpf.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : XamRibbonWindow
{
    private readonly IApplicationCommands _applicationCommands; 

    public MainWindow(IApplicationCommands applicationCommands)
    {
        InitializeComponent();

        _applicationCommands = applicationCommands;

        // Set the application theme to Office2013
        ThemeManager.ApplicationTheme = new Office2013Theme();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void XamOutlookBar_OnSelectedGroupChanged(object sender, RoutedEventArgs e)
    {
        var test = (sender as XamOutlookBar).SelectedGroup;
        var group = (sender as XamOutlookBar).SelectedGroup as IOutlookBarGroup;
        if (group != null)
        {
            _applicationCommands.NavigateCommand.Execute(group.DefaultNavigationPath);
        }
    }
}    