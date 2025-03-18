using Outlook.Core;
using Outlook.Core.Interfaces;
using Prism.Regions;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Windows;

namespace Outlook.Wpf.Core.Regions
{
    public class RegionManagerAwareBehavior : RegionBehavior
    {
        public const string BehaviorKey = "RegionManagerAwareBehavior";
        protected override void OnAttach()
        {
            if (Region.Name == RegionNames.ContentRegion)
            {
                Region.ActiveViews.CollectionChanged += OnActiveViews_CollectionChanged;
            }
        }

        private void OnActiveViews_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    InvokeOnRegionManagerAwareElement(item, x => x.RegionManager = Region.RegionManager);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    InvokeOnRegionManagerAwareElement(item, x => x.RegionManager = null);
                }
            }
        }

        private void InvokeOnRegionManagerAwareElement(object item, Action<IRegionManagerAware> value)
        {
            var rmAware = item as IRegionManagerAware;
            if (rmAware != null)
            {
                value(rmAware);
            }

            var rmAwareFrameworkElement = item as FrameworkElement;
            if (rmAwareFrameworkElement != null)
            {
                var rwAwareDataContext = rmAwareFrameworkElement.DataContext as IRegionManagerAware;
                if (rwAwareDataContext != null)
                {
                    value(rwAwareDataContext);
                }
            }
        }
    }
}
