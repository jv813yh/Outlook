using Outlook.Core.Interfaces;
using Outlook.Wpf.Core.Dialogs.Controls;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using Outlook.Core;

namespace Outlook.Wpf.Core.Dialogs
{

    /// <summary>
    /// Custom dialog service for the WPF application
    /// </summary>
    public class RegionDialogService : DialogServiceBase, IRegionDialogService
    {
        private readonly IRegionManager _regionManager;
        private readonly IContainerExtension _containerExtension;


        public RegionDialogService(IContainerExtension containerExtension,
            IRegionManager regionManager)
            : base(containerExtension)
        {
            _regionManager = regionManager;
            _containerExtension = containerExtension;
        }

        public void ShowRegionDialog(string regionName, 
            string viewName, 
            IDialogParameters dialogParameters,
            Action<IDialogResult> callback)
        {
            var window = _containerExtension.Resolve<RibbonDialogWindow>();
            // Create a new RegionManager for the dialog window because we use the same regions
            // and Prism does not allow to have the same region in multiple RegionManagers
            var newRegionManager = _regionManager.CreateRegionManager();
            RegionManager.SetRegionManager(window, newRegionManager);

            // Navigate for setting DataContext of the view
            newRegionManager.RequestNavigate(regionName, viewName);

            // Get Region
            IRegion region = newRegionManager.Regions[RegionNames.ContentRegion];
            // Get the active view
            var activeViews = region.ActiveViews.FirstOrDefault() as FrameworkElement;
            // Get the DataContext of the active view by casting it to IDialogAware
            IDialogAware dialogAware = activeViews.DataContext as IDialogAware;

            if (dialogAware != null)
            {
                // Insert parameters into the dialogAware object
                dialogAware.OnDialogOpened(dialogParameters);

                CancelEventHandler closingHandler = null;
                //IDialogAware dialogAware = ((FrameworkElement)window.MainRegion).DataContext as IDialogAware;

                // Handler for closing the dialog window
                Action<IDialogResult> requestCloseHandler = null;
                requestCloseHandler = (dialogResult) =>
                {
                    window.Result = dialogResult;
                    window.Close();
                };

                // Handler for verifying if the dialog can be closed
                closingHandler = (o, e) =>
                {

                    if (!dialogAware.CanCloseDialog())
                        e.Cancel = true;
                };
                window.Closing += closingHandler;

                //// RoutedEventHandler is a predefined delegate in WPF
                //// that is used to handle routed events (e.g. Loaded, Click).
                RoutedEventHandler loadedHandler = null;
                loadedHandler = (s, e) =>
                {
                    window.MainRegion.Loaded -= loadedHandler;

                    // Content is a reference to the View
                    //window.DataContext = view.DataContext;
                    // window.RibbonRegion.DataContext = view.DataContext;
                    window.MainRegion.DataContext = dialogAware;

                    // The DataContext is a reference to the ViewModel
                    dialogAware.RequestClose += requestCloseHandler;
                    //dialogAware.RequestClose += _ => window.Close();
                };

                window.MainRegion.Loaded += loadedHandler;


                // To avoid memory leak
                EventHandler closeHandler = null;
                closeHandler = (s, e) =>
                {
                    window.Closed -= closeHandler;
                    window.Closing -= closingHandler;

                    dialogAware.OnDialogClosed();
                    // todo: get dialog results

                    var dialogResults = window.Result;
                    if (dialogResults == null)
                    {
                        dialogResults = new DialogResult();
                    }

                    callback?.Invoke(dialogResults);


                    window.DataContext = null;
                    window.Content = null;
                    //window.MainRegion.DataContext = null;

                    newRegionManager.Regions.ToList().ForEach(r => _regionManager.Regions.Remove(r.Name));
                };

                window.Closed += closeHandler;

            }

            //newRegionManager.RequestNavigate(regionName, viewName);

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Show();
        }
    }
}
