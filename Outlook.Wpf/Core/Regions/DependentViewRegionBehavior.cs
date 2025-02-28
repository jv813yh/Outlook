using Outlook.Core.Attributes;
using Prism.Regions;
using System.Collections.Specialized;
using System.Resources;
using Outlook.Wpf.Models;
using Prism.Ioc;

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
        private readonly Dictionary<object, IList<DependentViewInfo>> _dependentViewCache;

        // For registration region behavior in Prism
        public const string BehaviorKey = "DependentViewRegionBehavior";

        public DependentViewRegionBehavior(IContainerExtension containerExtension)
        {
            _containerExtension = containerExtension;
            _dependentViewCache = new Dictionary<object, IList<DependentViewInfo>>();
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

                    // get the custom attributes of the view
                    var atts = GetCustomAttributes<DependentViewAttribute>(view.GetType());

                    foreach (var att in atts)
                    {
                        dependentViews.Add(CreateDependentViewInfo(att));
                    }

                    // add dependent views to the region
                    foreach (var dependentView in dependentViews)
                    {
                        if (Region.RegionManager.Regions.ContainsRegionWithName(dependentView.Region))
                        {
                            Region.RegionManager.Regions[dependentView.Region].Add(dependentView.View);
                        }
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {

            }
        }

        /// <summary>
        /// Get custom attributes 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IEnumerable<T> GetCustomAttributes<T>(Type type)
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
