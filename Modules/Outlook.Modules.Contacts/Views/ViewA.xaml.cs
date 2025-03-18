using Outlook.Core.Attributes;
using System.Windows.Controls;
using Outlook.Modules.Contacts.Menus;
using Outlook.Core;

namespace Outlook.Modules.Contacts.Views
{
    /// <summary>
    /// Interaction logic for ViewA.xaml
    /// </summary>
    /// 
    [DependentView(RegionNames.RibbonRegion, typeof(HomeTab))]
    public partial class ViewA : UserControl
    {
        public ViewA()
        {
            InitializeComponent();
        }
    }
}
