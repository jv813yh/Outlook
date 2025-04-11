using Outlook.Core;
using Outlook.Core.Attributes;
using Outlook.Modules.Mail.Menus;
using System.Windows.Controls;

namespace Outlook.Modules.Mail.Views
{
    /// <summary>
    /// Interaction logic for MessageReadOnlyView
    /// </summary>
    [DependentView(RegionNames.RibbonRegion, typeof(MessageReadOnlyTab))]
    public partial class MessageReadOnlyView : UserControl
    {
        public MessageReadOnlyView()
        {
            InitializeComponent();
        }
    }
}
