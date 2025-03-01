using Infragistics.Windows.Ribbon;
using Outlook.Core.Interfaces;

namespace Outlook.Modules.Mail.Menus
{
    /// <summary>
    /// Interaction logic for HomeTab.xaml
    /// </summary>
    public partial class HomeTab : RibbonTabItem, ISupportDataContext
    {
        public HomeTab()
        {
            InitializeComponent();
            // This way, the style defined in resources is automatically applied to this specific RibbonTabItem,
            // without the need to manually assign the style in each part of the code.
            //SetResourceReference(StyleProperty, typeof(RibbonTabItem));
        }
    }
}
