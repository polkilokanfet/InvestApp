using System;
using System.Collections.Specialized;
using Infragistics.Windows.Ribbon;
using Prism.Regions;

namespace InvestApp.Core.Region.RegionAdapters
{
    public class XamRibbonRegionAdapter : RegionAdapterBase<XamRibbon>
    {
        public XamRibbonRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory) : base(regionBehaviorFactory)
        {
        }

        protected override void Adapt(IRegion region, XamRibbon regionTarget)
        {
            if (region == null) throw new ArgumentNullException(nameof(region));
            if (regionTarget == null) throw new ArgumentNullException(nameof(regionTarget));

            region.ActiveViews.CollectionChanged += (sender, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                    {
                        foreach (object newView in args.NewItems)
                        {
                            if (newView is RibbonTabItem ribbonTabItem)
                            {
                                regionTarget.Tabs.Add(ribbonTabItem);
                            }
                        }
                        break;
                    }
                    case NotifyCollectionChangedAction.Remove:
                    {
                        foreach (object oldView in args.OldItems)
                        {
                            if (oldView is RibbonTabItem ribbonTabItem)
                            {
                                regionTarget.Tabs.Remove(ribbonTabItem);
                            }
                        }
                        break;
                    }
                }
            };
        }

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }
    }
}