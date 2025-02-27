using Outlook.Core.Attributes;
using Outlook.Core;
using System.Windows.Controls;
using Outlook.Modules.Contacts.Menus;

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
