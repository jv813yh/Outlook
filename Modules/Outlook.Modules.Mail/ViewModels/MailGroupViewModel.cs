using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using Outlook.Business;
using Outlook.Core.Interfaces;
using Prism.Commands;
using Prism.Mvvm;

namespace Outlook.Modules.Mail.ViewModels
{
    public class MailGroupViewModel : BindableBase
    {
        private readonly IApplicationCommands _applicationCommands;


        private DelegateCommand<NavigationItem> _selectedCommand;
        public DelegateCommand<NavigationItem> SelectedCommand
        => _selectedCommand ?? (_selectedCommand = new DelegateCommand<NavigationItem>(ExecuteSelectedCommand));

        private ObservableCollection<NavigationItem> _items;

        public ObservableCollection<NavigationItem> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public MailGroupViewModel(IApplicationCommands applicationCommands)
        {
            _items = new ObservableCollection<NavigationItem>();

            GenerateMenu();
            _applicationCommands = applicationCommands;
        }

        void ExecuteSelectedCommand(NavigationItem navigationItem)
        {
            if (navigationItem != null && 
                !string.IsNullOrEmpty(navigationItem.NavigationPath))
            {
                _applicationCommands.NavigateCommand.Execute(navigationItem.NavigationPath);
            }
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
