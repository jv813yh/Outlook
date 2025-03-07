using Infragistics.Controls.Menus;
using Infragistics.Windows.OutlookBar;
using Outlook.Business;
using Outlook.Core.Interfaces;

namespace Outlook.Modules.Mail.Menus
{
    /// <summary>
    /// Interaction logic for MailGroup.xaml
    /// </summary>
    ///
    /// OutlookBarGroup is navigation bar with multiple groups of items
    public partial class MailGroup : OutlookBarGroup, IOutlookBarGroup
    {
        public MailGroup()
        {
            InitializeComponent();
        }

        public string DefaultNavigationPath
        {
            get
            {
                // Get the selected item from the tree view when we go back to the mail list
                var item = DataTree.SelectionSettings.SelectedNodes[0] as XamDataTreeNode;
                if (item != null)
                {
                    var navigationPath = (item.Data as NavigationItem).NavigationPath;

                    if (!string.IsNullOrEmpty(navigationPath))
                    {
                        return navigationPath;
                    }
                }

                // Default navigation path
                return FolderParameters.GetNavigationPath(FolderParameters.MailListPath, FolderParameters.FolderKey ,FolderParameters.Inbox);
            }
        }
    }
}
