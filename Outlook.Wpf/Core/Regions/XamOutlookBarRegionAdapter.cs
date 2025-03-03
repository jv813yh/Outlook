using Infragistics.Windows.OutlookBar;
using Prism.Regions;
using System.Collections.Specialized;

namespace Outlook.Wpf.Core.Regions
{
    /*
     * RegionAdapterBase<T> defines how to add and remove views to these elements (UI controls),
     * that WPF does not know how to manage.
     */

    public class XamOutlookBarRegionAdapter : RegionAdapterBase<XamOutlookBar>
    {
        public XamOutlookBarRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory) 
            : base(regionBehaviorFactory)
        {
        }

        /// <summary>
        /// Method call automatically when the region is initialized.
        /// Allows you to customize the region to work properly with a specific UI element (XamOutlookBar).
        /// </summary>
        /// <param name="region"> maintains a list of registered views (Views) </param>
        /// <param name="regionTarget"> UI control </param>
        protected override void Adapt(IRegion region, XamOutlookBar regionTarget)
        {
            // CollectionChanged for all views in the region
            region.Views.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (var item in e.NewItems)
                    {
                        var group = item as OutlookBarGroup;
                        if (group != null)
                        {
                            // Add group (customer UI control) to the OutlookBar
                            regionTarget.Groups.Add(group);

                            if (regionTarget.Groups[0] == item)
                            {
                                regionTarget.SelectedGroup = group;
                            }
                        }
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (OutlookBarGroup group in e.OldItems)
                    {
                        if (group != null)
                        {
                            regionTarget.Groups.Remove(group);
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Method call automatically when the region is initialized.
        /// Determines the type of region to be created.
        /// </summary>
        /// <returns></returns>
        protected override IRegion CreateRegion()
         => new SingleActiveRegion();
    }
}
