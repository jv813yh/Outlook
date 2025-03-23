using Prism.Services.Dialogs;

namespace Outlook.Wpf.Core.Dialogs.Controls
{
    /// <summary>
    /// Interaction logic for RibbonDialogWindow.xaml
    /// </summary>
    public partial class RibbonDialogWindow : IDialogWindow 
    {
        public RibbonDialogWindow()
        {
            InitializeComponent();
        }

        public IDialogResult Result { get; set; }




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
