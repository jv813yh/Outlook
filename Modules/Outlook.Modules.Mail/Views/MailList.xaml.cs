using Outlook.Core;
using Outlook.Core.Attributes;
using Outlook.Core.Interfaces;
using Outlook.Modules.Mail.Menus;
using System.Windows.Controls;

namespace Outlook.Modules.Mail.Views
{
    /// <summary>
    /// Interaction logic for MailList
    /// </summary>
    [DependentView(RegionNames.RibbonRegion, typeof(HomeTab))]
    public partial class MailList : UserControl, ISupportDataContext
    {
        public MailList()
        {
            InitializeComponent();
        }
    }
}
