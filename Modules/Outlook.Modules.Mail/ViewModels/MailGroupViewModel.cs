using Outlook.Business;
using Outlook.Core.Interfaces;
using Outlook.Core.ViewModels;
using Prism.Commands;
using System.Collections.ObjectModel;

namespace Outlook.Modules.Mail.ViewModels
{
    public class MailGroupViewModel : ViewModelBase
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
            _applicationCommands = applicationCommands;

            GenerateMenu();
        }

        void ExecuteSelectedCommand(NavigationItem navigationItem)
        {
            if (navigationItem != null && 
                !string.IsNullOrEmpty(navigationItem.NavigationPath))
            {
                // Navigation according item
                _applicationCommands.NavigateCommand.Execute(navigationItem.NavigationPath);
            }
        }


        void GenerateMenu()
        {
            
            // Create the root item
            var root = new NavigationItem()
            {
                Caption = FolderParameters.PersonalFolder,
                NavigationPath = "MailList?id=Default",
                IsExpanded = true
            };

            // Items ...
            root.Items.Add(new NavigationItem()
            {
                Caption = FolderParameters.Inbox,
                NavigationPath = FolderParameters.GetNavigationPath(FolderParameters.MailListPath, 
                                 FolderParameters.FolderKey,
                                 FolderParameters.Inbox)
            });

            root.Items.Add(new NavigationItem()
            {
                Caption = FolderParameters.Deleted,
                NavigationPath = FolderParameters.GetNavigationPath(FolderParameters.MailListPath, 
                                 FolderParameters.FolderKey,
                                 FolderParameters.Deleted)
            });

            root.Items.Add(new NavigationItem()
            {
                Caption = FolderParameters.Sent,
                NavigationPath = FolderParameters.GetNavigationPath(FolderParameters.MailListPath, 
                                 FolderParameters.FolderKey, 
                                 FolderParameters.Sent)
            });


            Items.Add(root);
        }
    }
}
