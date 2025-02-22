using Infragistics.Themes;
using Infragistics.Windows.Ribbon;

namespace Outlook.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : XamRibbonWindow
{
    public MainWindow()
    {
        InitializeComponent();

        // Set the application theme to Office2013
        ThemeManager.ApplicationTheme = new Office2013Theme();
    }
}