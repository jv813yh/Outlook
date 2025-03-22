using Infragistics.Windows.Ribbon;

namespace Outlook.Wpf.Core.Dialogs.Controls
{
    /// <summary>
    /// Interaction logic for RibbonDialogWindow.xaml
    /// </summary>
    public partial class RibbonDialogWindow : XamRibbonWindow
    {
        public RibbonDialogWindow()
        {
            InitializeComponent();
        }

        //public new object DataContext
        //{
        //    get
        //    {
        //        return ((FrameworkElement)MainRegion.DataContext).DataContext;
        //    }
        //    set => base.DataContext = value;
        //}

    }
}
