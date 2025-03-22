﻿using Outlook.Core;
using Outlook.Core.Interfaces;
using Outlook.Wpf.Core.Dialogs.Controls;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using Outlook.Modules.Mail.Views;

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

        public void ShowRegionDialog(string regionName, string viewName)
        {
            var window = _containerExtension.Resolve<RibbonDialogWindow>();

            var newRegionManager = _regionManager.CreateRegionManager();
            RegionManager.SetRegionManager(window, newRegionManager);


            //RegionManagerAware.SetRegionManagerAware(window, newRegionManager);

            //newRegionManager.RequestNavigate(regionName, viewName);

            //IRegion region = newRegionManager.Regions[RegionNames.ContentRegion];
            //var activeViews = region.ActiveViews.FirstOrDefault() as FrameworkElement;
            //IDialogAware dialogAware = activeViews.DataContext as IDialogAware;

            CancelEventHandler closingHandler = null;

            newRegionManager.Regions[regionName].ActiveViews.CollectionChanged += (sender, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (var item in e.NewItems)
                    {
                        IDialogAware dialogAware = ((FrameworkElement)item).DataContext as IDialogAware;

                        if (window != null)
                        {

                            Action<IDialogResult> requestCloseHandler = null;
                            requestCloseHandler = (r) =>
                            {
                                window.Close();
                            };

                            closingHandler = (o, e) =>
                            {

                                if (dialogAware != null)
                                {
                                    if (!dialogAware.CanCloseDialog())
                                        e.Cancel = true;
                                }
                            };
                            window.Closing += closingHandler;

                            //// RoutedEventHandler is a predefined delegate in WPF
                            //// that is used to handle routed events (e.g. Loaded, Click).
                            RoutedEventHandler loadedHandler = null;
                            loadedHandler = (s, e) =>
                            {
                                window.MainRegion.Loaded -= loadedHandler;

                                // Content is a reference to the View
                                if (dialogAware != null)
                                {
                                    //window.DataContext = view.DataContext;
                                    // window.RibbonRegion.DataContext = view.DataContext;
                                    window.MainRegion.DataContext = dialogAware;
                                }

                                // The DataContext is a reference to the ViewModel
                                if (dialogAware != null)
                                {
                                    dialogAware.RequestClose += requestCloseHandler;
                                    //dialogAware.RequestClose += _ => window.Close();
                                }
                            };

                            window.MainRegion.Loaded += loadedHandler;


                            // To avoid memory leak
                            EventHandler closeHandler = null;
                            closeHandler = (s, e) =>
                            {
                                window.Closed -= closeHandler;
                                window.Closing -= closingHandler;

                                window.DataContext = null;
                                window.Content = null;
                                //window.MainRegion.DataContext = null;

                                newRegionManager.Regions.ToList().ForEach(r => _regionManager.Regions.Remove(r.Name));
                            };

                            window.Closed += closeHandler;

                        }
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (var item in e.OldItems)
                    {
                        //IDialogAware dialogAware = ((FrameworkElement)item).DataContext as IDialogAware;
                        //window.Closing -= closingHandler;
                    }
                }
            };


            //CancelEventHandler closingHandler = null;
            //closingHandler = (o, e) =>
            //{

            //    if (window.MainRegion.DataContext is IDialogAware da)
            //    {
            //        if (!da.CanCloseDialog())
            //            e.Cancel = true;
            //    }
            //};
            //window.Closing += closingHandler;

            ////// RoutedEventHandler is a predefined delegate in WPF
            ////// that is used to handle routed events (e.g. Loaded, Click).
            //RoutedEventHandler loadedHandler = null;
            //loadedHandler = (s, e) =>
            //{
            //    window.MainRegion.Loaded -= loadedHandler;

            //    // Content is a reference to the View
            //    if (window.MainRegion.Content is FrameworkElement view)
            //    {
            //        //window.DataContext = view.DataContext;
            //        // window.RibbonRegion.DataContext = view.DataContext;
            //        window.MainRegion.DataContext = view.DataContext;
            //    }

            //    // The DataContext is a reference to the ViewModel
            //    if (window.MainRegion.DataContext is IDialogAware da)
            //    {
            //        da.RequestClose += _ => window.Close();
            //    }
            //};

            //window.MainRegion.Loaded += loadedHandler;


            //// To avoid memory leak
            //EventHandler closeHandler = null;
            //closeHandler = (s, e) =>
            //{
            //    window.Closed -= closeHandler;
            //    window.Closing -= closingHandler;

            //    window.DataContext = null;
            //    window.Content = null;
            //    window.MainRegion.DataContext = null;

            //    newRegionManager.Regions.ToList().ForEach(r => _regionManager.Regions.Remove(r.Name));
            //};

            //window.Closed += closeHandler;

            newRegionManager.RequestNavigate(regionName, viewName);

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Show();
        }

      
    }
}
