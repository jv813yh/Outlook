using Outlook.Core.Attributes;
using Prism.Regions;
using System.Collections.Specialized;
using System.Resources;
using Outlook.Wpf.Models;
using Prism.Ioc;
using System.Windows;
using Outlook.Core.Interfaces;

namespace Outlook.Wpf.Core.Regions
{
    /*
     * RegionBehavior class that allows you to add specific behavior to regions in your application.
     * It helps extend the functionality of RegionManager and customize the way regions work.
     *
     * Each region in Prism can have special behaviors, for example:
     * BindRegionContextToDependencyObjectBehavior - automatically sets the DataContext for regional views.
     * AutoPopulateRegionBehavior - automatically adds Views to a region on initialization.
     *
     * Custom region behavior that allows you to manage dependent views in a region.
     *
     */
    public class DependentViewRegionBehavior : RegionBehavior
    {
        private readonly IContainerExtension _containerExtension;
        private readonly Dictionary<object, List<DependentViewInfo>> _dependentViewCache;

        // For registration region behavior in Prism
        public const string BehaviorKey = "DependentViewRegionBehavior";

        public DependentViewRegionBehavior(IContainerExtension containerExtension)
        {
            _containerExtension = containerExtension;
            _dependentViewCache = new Dictionary<object, List<DependentViewInfo>>();
        }

        /// <summary>
        /// It is called when the RegionBehavior is attached to a region.
        /// </summary>
        protected override void OnAttach()
        {
            Region.ActiveViews.CollectionChanged += ActiveViews_CollectionChanged;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActiveViews_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var view in e.NewItems)
                {
                    var dependentViews = new List<DependentViewInfo>();

                    if (_dependentViewCache.ContainsKey(view))
                    {
                        dependentViews = _dependentViewCache[view];
                    }
                    else
                    {
                        // get the custom attributes of the view
                        var atts = GetCustomAttributes<DependentViewAttribute>(view.GetType());

                        foreach (var att in atts)
                        {
                            var info = CreateDependentViewInfo(att);

                            // Set the DataContext of the dependent view to the DataContext of the view
                            if (info.View is ISupportDataContext infoDC &&
                                view is ISupportDataContext viewDC)
                            {
                                infoDC.DataContext = viewDC.DataContext;
                            }

                            dependentViews.Add(info);
                        }

                        // add to cache
                        _dependentViewCache.Add(view, dependentViews);
                    }

                    // add dependent views to the region
                    foreach (var dependentView in dependentViews)
                    {
                        if (Region.RegionManager.Regions.ContainsRegionWithName(dependentView.Region))
                        {
                            // add dependent views to the region
                            Region.RegionManager.Regions[dependentView.Region].Add(dependentView.View);
                        }
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var oldViews in e.OldItems)
                {
                    if (_dependentViewCache.ContainsKey(oldViews))
                    {
                        _dependentViewCache[oldViews].ForEach(dependentView =>
                        {
                            if (Region.RegionManager.Regions.ContainsRegionWithName(dependentView.Region))
                            {
                                // remove dependent views from the region
                                Region.RegionManager.Regions[dependentView.Region].Remove(dependentView.View);
                            }

                            if (!ShouldKeepAlive(oldViews))
                            {
                                _dependentViewCache.Remove(oldViews);
                            }
                        });
                    }
                }
            }
        }

        private bool ShouldKeepAlive(object oldViews)
        {
            var lifeTime = GetViewOrDataContextLifeTime(oldViews);
            if (lifeTime != null)
            {
                return lifeTime.KeepAlive;
            }

            return true;
        }

        /// <summary>
        /// IRagionMemberLifetime determines whether a View instance
        /// remains in memory or is destroyed when navigating between Views in regions.
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        IRegionMemberLifetime? GetViewOrDataContextLifeTime(object view)
        {
            if (view is IRegionMemberLifetime lifetime)
            {
                return lifetime;
            }

            if (view is FrameworkElement frameworkElement)
            {
                return frameworkElement.DataContext as IRegionMemberLifetime;
            }

            return null;
        }

        /*
         * Object  
             └─ DispatcherObject  -  threading, marshaling, and synchronization
                 └─ DependencyObject  -  data binding, resources, styles, templates
                     └─ Visual  -  rendering, hit testing, and input
                         └─ UIElement  -  layout, input, events, automation
                             └─ FrameworkElement -  layout, data binding and events...
         *
         */

        /// <summary>
        /// Get custom attributes 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        //private IEnumerable<T> GetCustomAttributes<T>(Type type)
        // => type.GetCustomAttributes(typeof(T), true).OfType<T>();

        private IEnumerable<T> GetCustomAttributes<T>(Type type)
            => type.GetCustomAttributes(typeof(T), true).OfType<T>();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dependentViewAttribute"></param>
        /// <returns></returns>
        DependentViewInfo CreateDependentViewInfo(DependentViewAttribute dependentViewAttribute)
        {
            var returnInfo = new DependentViewInfo();
            returnInfo.Region = dependentViewAttribute.Region;
            returnInfo.View = _containerExtension.Resolve(dependentViewAttribute.Type);

            return returnInfo;
        }
    }
}
