using Infragistics.Windows.Ribbon;
using Outlook.Core;
using Outlook.Modules.Mail.Views;
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
        }

        public IDialogResult Result { get; set; }

        public void Initialize(string name, IDialogParameters parameters)
        {
            // I use the same regions as the main window application but that is not allowed by defualt,
            // so I need to create a new region manager and then I can use the same regions or use different regions
            var regionManagerMessageDialog = _regionManager.CreateRegionManager();
            RegionManager.SetRegionManager(this, regionManagerMessageDialog);

            // Register navigation for the MessageDialogView
            regionManagerMessageDialog.RequestNavigate(RegionNames.ContentRegion, name);
        }
    }
}
