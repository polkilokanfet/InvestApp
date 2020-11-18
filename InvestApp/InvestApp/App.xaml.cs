using Prism.Ioc;
using InvestApp.Views;
using System.Windows;
using Infragistics.Windows.OutlookBar;
using Infragistics.Windows.Ribbon;
using InvestApp.Core.ApplicationCommands;
using InvestApp.Core.Region;
using InvestApp.Core.Region.RegionAdapters;
using InvestApp.Domain.Interfaces;
using InvestApp.Domain.Services;
using InvestApp.Domain.Services.DataBaseAccess;
using Prism.Modularity;
using InvestApp.Modules.ModuleName;
using InvestApp.Modules.ModuleName.ViewModels;
using InvestApp.Services;
using InvestApp.Services.DataBaseAccess;
using InvestApp.Services.FinancialModelingPrepService;
using InvestApp.Services.TinkoffOpenApiService;
using Microsoft.EntityFrameworkCore;
using Prism.Regions;

namespace InvestApp
{
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<FinancialModelingHttpClientFactory>(new FinancialModelingHttpClientFactory("54f54a0a0365d2cc288f3c5b02e709b5"));

            containerRegistry.RegisterSingleton<IApplicationCommands, ApplicationCommands>();
            containerRegistry.RegisterSingleton<DbContext, InvestAppDbContext>();
            containerRegistry.RegisterSingleton<IUnitOfWork, UnitOfWork>();
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();
            containerRegistry.RegisterSingleton<IFinancialModelingPrepService, FinancialModelingService>();
            containerRegistry.RegisterSingleton<AssetsViewModel>();

            //string tokenSandbox = @"t.6S_c2O6mXa8dkH6aHkZvACVa_jeRyRAR-q_k7zmF-0qbyjW2vEX09a01wCJegeJ6aV38vPfl5jsvw_taUpbuUQ";
            string token = @"t.CdFxair_TTItA-48yTBZU5-XmV2bV63uL5AFFit4yUxcRC0HOczQDNsNKgTEy8hF0jXvWCDaqvyI35pXM7FecA";
            containerRegistry.RegisterInstance<IMarketStore>(
                new MarketStore(
                    token, 
                    false, 
                    Container.Resolve<IUnitOfWork>(), 
                    Container.Resolve<IMessageService>(), 
                    Container.Resolve<IFinancialModelingPrepService>()));

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
