using System;
using System.Collections.Specialized;
using Infragistics.Windows.Ribbon;
using Prism.Regions;

namespace InvestApp.Core.RegionAdapters
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
                        foreach (object view in args.NewItems)
                        {
                            if (view is RibbonTabItem ribbonTabItem)
                            {
                                regionTarget.Tabs.Add(ribbonTabItem);
                            }
                        }
                        break;
                    }
                    case NotifyCollectionChangedAction.Remove:
                    {
                        foreach (object view in args.OldItems)
                        {
                            if (view is RibbonTabItem ribbonTabItem)
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