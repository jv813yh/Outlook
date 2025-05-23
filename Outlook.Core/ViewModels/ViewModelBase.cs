﻿using Prism.Mvvm;
using Prism.Regions;

namespace Outlook.Core.ViewModels
{
    /*
     * ViewModelBase base class for all view models in the application.
     *
     * BindableBase:
     * Is used in Prism to implement INotifyPropertyChanged
     *
     * IConfirmNavigationRequest:
     * Is used in Prism to confirm a navigation before it is actually executed.
     * Useful when you want to prevent leaving a page or view without confirmation
     *
     */
    public class ViewModelBase : BindableBase, IConfirmNavigationRequest
    {
        public virtual void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            // Default true, allow navigation
            continuationCallback(true);
        }

        // Reuse view instance by default
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
         => true;

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
    }
}
