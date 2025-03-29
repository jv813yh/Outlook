using Infragistics.Controls.Editors;
using Outlook.Core;
using Outlook.Core.Attributes;
using Outlook.Core.Interfaces;
using Outlook.Modules.Mail.Menus;
using System.Windows.Controls;

namespace Outlook.Modules.Mail.Views
{
    /// <summary>
    /// Interaction logic for MessageView
    /// </summary>
    //[DependentView(RegionNames.RibbonRegion, typeof(HomeTab))]
    [DependentView(RegionNames.RibbonRegion, typeof(MessageTab))]
    public partial class MessageDialogView : UserControl, ISupportDataContext, ISupportRichText
    {
        public MessageDialogView()
        {
            InitializeComponent();
        }

        public XamRichTextEditor RichTextEditor 
        {
            get => richTextEditor;
            set => richTextEditor = value;
        }
    }
}
