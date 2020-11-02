using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace InvestApp.Core.Mvvm
{
    public abstract class ModuleBase : IModule
    {
        protected IContainerProvider Container { get; private set; }
        protected IRegionManager RegionManager { get; }

        protected ModuleBase(IRegionManager regionManager)
        {
            RegionManager = regionManager;
        }

        public abstract void RegisterTypes(IContainerRegistry containerRegistry);
        protected abstract void ResolveOutlookGroup();

        public virtual void OnInitialized(IContainerProvider containerProvider)
        {
            Container = containerProvider;
            ResolveOutlookGroup();
        }
    }
}