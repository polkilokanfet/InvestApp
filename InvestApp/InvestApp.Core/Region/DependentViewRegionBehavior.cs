using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using InvestApp.Infrastructure.Attributes;
using Prism.Regions;
using InvestApp.Infrastructure.Extansions;
using Prism.Ioc;

namespace InvestApp.Core.Region
{
    public class DependentViewRegionBehavior : RegionBehavior
    {
        public const string BehaviorKey = "DependentViewRegionBehavior";
        private readonly IContainerExtension _container;
        private readonly Dictionary<object, List<DependentViewInfo>> _dependentViewsCache = new Dictionary<object, List<DependentViewInfo>>();

        public DependentViewRegionBehavior(IContainerExtension container)
        {
            _container = container;
        }

        protected override void OnAttach()
        {
            Region.ActiveViews.CollectionChanged +=
                (sender, args) =>
                {
                    if (args.Action == NotifyCollectionChangedAction.Add)
                    {
                        foreach (var newView in args.NewItems)
                        {
                            var dependentViews = new List<DependentViewInfo>();

                            if (_dependentViewsCache.ContainsKey(newView))
                            {
                                dependentViews = _dependentViewsCache[newView];
                            }
                            else
                            {
                                var attributes = newView.GetType().GetCustomAttributes<DependentViewAttribute>();
                                foreach (var attribute in attributes)
                                {
                                    var dependentViewInfo = new DependentViewInfo(attribute.Region, _container.Resolve(attribute.Type));
                                    dependentViews.Add(dependentViewInfo);

                                    //внедрение в зависимый вид контекста основного вида
                                    if (attribute.HasSameDataContext)
                                    {
                                        if (newView is FrameworkElement mainView && dependentViewInfo.View is FrameworkElement dependentView)
                                            dependentView.DataContext = mainView.DataContext;
                                    }
                                }

                                _dependentViewsCache.Add(newView, dependentViews);
                            }

                            dependentViews.ForEach(x => Region.RegionManager.Regions[x.Region].Add(x.View));
                        }

                    }
                    else if (args.Action == NotifyCollectionChangedAction.Remove)
                    {
                        foreach (var oldView in args.OldItems)
                        {
                            if (_dependentViewsCache.ContainsKey(oldView))
                            {
                                var dependentViews = _dependentViewsCache[oldView];
                                dependentViews.ForEach(x => Region.RegionManager.Regions[x.Region].Remove(x.View));

                                //если вид нужно удалить
                                if (!ShouldKeepAlive(oldView))
                                    _dependentViewsCache.Remove(oldView);
                            }
                        }
                    }
                };
        }

        private bool ShouldKeepAlive(object oldView)
        {
            var regionLifetime = GetViewOrDataContextLifeTime(oldView);
            if (regionLifetime != null)
                return regionLifetime.KeepAlive;

            return true;
        }

        IRegionMemberLifetime GetViewOrDataContextLifeTime(object view)
        {
            if (view is IRegionMemberLifetime regionLifetime)
                return regionLifetime;

            if (view is FrameworkElement fe)
                return fe.DataContext as IRegionMemberLifetime;

            return null;
        }


        public class DependentViewInfo
        {
            public object View { get; }
            public string Region { get; }

            public DependentViewInfo(string region, object view)
            {
                Region = region;
                View = view;
            }
        }

    }
}