using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using Outlook.Business;
using Prism.Mvvm;

namespace Outlook.Modules.Mail.ViewModels
{
    public class MailGroupViewModel : BindableBase
    {
        private ObservableCollection<NavigationItem> _items;

        public ObservableCollection<NavigationItem> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public MailGroupViewModel()
        {
            _items = new ObservableCollection<NavigationItem>();

            GenerateMenu();
        }

        void GenerateMenu()
        {
            // Create the root item
            var root = new NavigationItem()
            {
                Caption = "Personal Folders",
                NavigationPath = "MailList"
            };

            // Items ...
            root.Items.Add(new NavigationItem()
            {
                Caption = "Inbox",
                NavigationPath = ""
            });

            root.Items.Add(new NavigationItem()
            {
                Caption = "Deleted",
                NavigationPath = ""
            });

            root.Items.Add(new NavigationItem()
            {
                Caption = "Sent",
                NavigationPath = ""
            });


            Items.Add(root);
        }
    }
}
