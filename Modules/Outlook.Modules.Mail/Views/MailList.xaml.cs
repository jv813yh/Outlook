using System.Windows.Controls;
using Outlook.Core;
using Outlook.Core.Attributes;
using Outlook.Modules.Mail.Menus;

namespace Outlook.Modules.Mail.Views
{
    /// <summary>
    /// Interaction logic for MailList
    /// </summary>
    [DependentView(RegionNames.RibbonRegion, typeof(HomeTab))]
    public partial class MailList : UserControl
    {
        public MailList()
        {
            InitializeComponent();
        }
    }
}
