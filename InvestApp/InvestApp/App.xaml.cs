using Prism.Ioc;
using InvestApp.Views;
using System.Windows;
using Infragistics.Windows.OutlookBar;
using Infragistics.Windows.Ribbon;
using InvesApp.Services.Tinkoff;
using InvestApp.Core.ApplicationCommands;
using InvestApp.Core.Region;
using InvestApp.Core.Region.RegionAdapters;
using InvestApp.Domain.Services;
using Prism.Modularity;
using InvestApp.Modules.ModuleName;
using InvestApp.Services.FinancialModelingPrepService;
using Prism.Regions;

namespace InvestApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IApplicationCommands, ApplicationCommands>();
            containerRegistry.RegisterSingleton<IRepository, TinkoffRepository>();
            containerRegistry.RegisterSingleton<IFinancialModelingPrepService, FinancialModelingService>();
            containerRegistry.RegisterInstance(typeof(FinancialModelingHttpClientFactory), new FinancialModelingHttpClientFactory("54f54a0a0365d2cc288f3c5b02e709b5"));
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ModuleNameModule>();
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
            regionAdapterMappings.RegisterMapping(typeof(XamOutlookBar), Container.Resolve<XamOutlookBarRegionAdapter>());
            regionAdapterMappings.RegisterMapping(typeof(XamRibbon), Container.Resolve<XamRibbonRegionAdapter>());
        }

        protected override void ConfigureDefaultRegionBehaviors(IRegionBehaviorFactory regionBehaviors)
        {
            base.ConfigureDefaultRegionBehaviors(regionBehaviors);
            regionBehaviors.AddIfMissing(DependentViewRegionBehavior.BehaviorKey, typeof(DependentViewRegionBehavior));
        }
    }
}
