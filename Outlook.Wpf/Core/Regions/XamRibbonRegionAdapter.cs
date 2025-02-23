using Infragistics.Windows.Ribbon;
using Prism.Regions;
using System.Collections.Specialized;

namespace Outlook.Wpf.Core.Regions
{
    public class XamRibbonRegionAdapter : RegionAdapterBase<XamRibbon>
    {
        public XamRibbonRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory) 
            : base(regionBehaviorFactory)
        {
        }

        protected override void Adapt(IRegion region, XamRibbon regionTarget)
        {
            if (region == null)
            {
                throw new ArgumentNullException(nameof(region));
            }

            if (regionTarget == null)
            {
                throw new ArgumentNullException(nameof(regionTarget));
            }

            region.Views.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (var item in e.NewItems)
                    {
                        if (item != null)
                        {
                            if (item is RibbonTabItem ribbonItem)
                            {
                                regionTarget.Tabs.Add(ribbonItem);
                            }
                        }
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach(var item in e.OldItems)
                    {
                        if (item != null)
                        {
                            if(item is RibbonTabItem ribbonItem)
                            {
                                regionTarget.Tabs.Remove(ribbonItem);
                            }
                        }
                    }
                }
            };
        }

        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }
    }
}
