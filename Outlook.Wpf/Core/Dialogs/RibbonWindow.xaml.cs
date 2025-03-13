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
    public partial class RibbonWindow : XamRibbonWindow, IDialogWindow
    {
        private readonly IRegionManager _regionManager;
        public RibbonWindow(IRegionManager regionManager)
        {
            InitializeComponent();
            _regionManager = regionManager;

            ThemeManager.ApplicationTheme = new Office2013Theme();
        }

        public IDialogResult Result { get; set; }

        public void Initialize(string name, IDialogParameters parameters)
        {
            // I use the same regions as the main window application but that is not allowed by defualt,
            // so I need to create a new region manager and then I can use the same regions or
            // you can use different regions with the same region manager
            var regionManagerMessageDialog = _regionManager.CreateRegionManager();
            RegionManager.SetRegionManager(this, regionManagerMessageDialog);

            // Register navigation for the MessageDialogView
            regionManagerMessageDialog.RequestNavigate(RegionNames.ContentRegion, name);
        }
    }
}
