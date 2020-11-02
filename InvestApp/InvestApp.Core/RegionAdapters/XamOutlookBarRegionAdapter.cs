using System;
using System.Collections.Specialized;
using Infragistics.Windows.OutlookBar;
using Prism.Regions;

namespace InvestApp.Core.RegionAdapters
{
    public class XamOutlookBarRegionAdapter : RegionAdapterBase<XamOutlookBar>
    {
        public XamOutlookBarRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory) : base(regionBehaviorFactory)
        {
        }

        protected override void Adapt(IRegion region, XamOutlookBar regionTarget)
        {
            if (region == null) 
                throw new ArgumentNullException(nameof(region));
            
            if (regionTarget == null) 
                throw new ArgumentNullException(nameof(regionTarget));

            region.ActiveViews.CollectionChanged += (sender, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                    {
                        foreach (OutlookBarGroup newGroup in args.NewItems)
                        {
                            regionTarget.Groups.Add(newGroup);

                            //выделение первой добавленной группы
                            if (Equals(regionTarget.Groups[0], newGroup))
                            {
                                regionTarget.SelectedGroup = newGroup;
                            }
                        }
                        break;
                    }
                    case NotifyCollectionChangedAction.Remove:
                    {
                        foreach (OutlookBarGroup oldGroup in args.OldItems)
                        {
                            regionTarget.Groups.Remove(oldGroup);
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