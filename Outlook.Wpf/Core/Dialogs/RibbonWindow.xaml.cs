using Infragistics.Themes;
using Infragistics.Windows.Ribbon;
using Outlook.Core;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace Outlook.Wpf.Core.Dialogs
{
    /// <summary>
    /// Interaction logic for RibbonWindow.xaml
    /// </summary>
    public partial class RibbonWindow : XamRibbonWindow
    {
        public RibbonWindow()
        {
            InitializeComponent();

            ThemeManager.ApplicationTheme = new Office2013Theme();
        }
    }
}
