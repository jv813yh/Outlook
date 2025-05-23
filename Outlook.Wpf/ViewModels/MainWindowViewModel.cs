﻿using Outlook.Core;
using Outlook.Core.Interfaces;
using Outlook.Core.ViewModels;
using Prism.Commands;
using Prism.Regions;
using System.Diagnostics;

namespace Outlook.Wpf.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IRegionManager _regionManager;

        private MessageViewModel _errorMessageViewModel;
        public MessageViewModel ErrorMessageViewModel
        {
            get => _errorMessageViewModel;
            set => SetProperty(ref _errorMessageViewModel, value);
        }


        private DelegateCommand<string> _navigateCommand;
        public DelegateCommand<string> NavigateCommand =>
            _navigateCommand ?? (_navigateCommand = new DelegateCommand<string>(ExecuteNavigateCommand));


        public MainWindowViewModel(IRegionManager regionManager, IApplicationCommands applicationCommands)
        {
            _regionManager = regionManager;
            // Register composite command for navigation
            applicationCommands.NavigateCommand.RegisterCommand(NavigateCommand);
            _errorMessageViewModel = new MessageViewModel();
        }

        /// <summary>
        /// Base navigation in Main Window according the selected email to ContentControl
        /// </summary>
        /// <param name="parameter"></param>
        /// <exception cref="ArgumentNullException"></exception>
        private void ExecuteNavigateCommand(string navigationPath)
        {
            _errorMessageViewModel.IsActive = false;
            _errorMessageViewModel.Message = string.Empty;

            if (string.IsNullOrEmpty(navigationPath))
            {
                throw new ArgumentNullException(nameof(navigationPath));
            }

            try
            {
                _regionManager.RequestNavigate(RegionNames.ContentRegion, navigationPath);
            }
            catch (Exception e)
            {
                _errorMessageViewModel.IsActive = true;
                _errorMessageViewModel.Message = "Error during navigation, please log file";
                Debug.WriteLine(e);
            }
        }
    }
}
