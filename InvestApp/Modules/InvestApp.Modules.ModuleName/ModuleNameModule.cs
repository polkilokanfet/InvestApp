using InvestApp.Core;
using InvestApp.Core.Mvvm;
using InvestApp.Modules.ModuleName.Menus;
using InvestApp.Modules.ModuleName.Views;
using Prism.Ioc;
using Prism.Regions;

namespace InvestApp.Modules.ModuleName
{
    public class ModuleNameModule : ModuleBase
    {
        public ModuleNameModule(IRegionManager regionManager) : base(regionManager)
        {
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ViewA>();
            containerRegistry.RegisterForNavigation<ViewB>();
        }

        protected override void ResolveOutlookGroup()
        {
            //RegionManager.Regions[RegionNames.OutlookBarGroupsRegion].Add(Container.Resolve<ModuleMenu>());
            RegionManager.AddToRegion(RegionNames.OutlookBarGroupsRegion, Container.Resolve<ModuleMenu>());
        }

    }
}