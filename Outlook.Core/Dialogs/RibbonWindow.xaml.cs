using System.Windows;
using Infragistics.Themes;
using Infragistics.Windows.Ribbon;

namespace Outlook.Core.Dialogs
{
    /// <summary>
    /// Interaction logic for RibbonWindow.xaml
    /// </summary>
    public partial class RibbonWindow : XamRibbonWindow
    {
        public RibbonWindow()
        {
            InitializeComponent();
            //ThemeManager.ApplicationTheme = new Office2013Theme();
        }
    }
}
