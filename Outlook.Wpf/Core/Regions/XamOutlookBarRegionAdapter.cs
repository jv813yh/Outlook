using Infragistics.Windows.OutlookBar;
using Prism.Regions;

namespace Outlook.Wpf.Core.Regions
{
    public class XamOutlookBarRegionAdapter : RegionAdapterBase<XamOutlookBar>
    {
        public XamOutlookBarRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory) 
            : base(regionBehaviorFactory)
        {
        }

        protected override void Adapt(IRegion region, XamOutlookBar regionTarget)
        {
            region.Views.CollectionChanged += (s, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    foreach (var item in e.NewItems)
                    {
                        var group = item as OutlookBarGroup;
                        if (group != null)
                        {
                            regionTarget.Groups.Add(group);

                            if (regionTarget.Groups[0] == item)
                            {
                                regionTarget.SelectedGroup = group;
                            }
                        }
                    }
                }
                else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
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

        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }
    }
}
