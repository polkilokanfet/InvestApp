﻿using InvestApp.Core.Attributes;
using InvestApp.Core.Mvvm;
using InvestApp.Modules.ModuleName.Tabs;
using Prism.Events;
using Prism.Regions;

namespace InvestApp.Modules.ModuleName.Views
{
    [RibbonTab(typeof(ViewATab))]
    public partial class ViewA : ViewBase
    {
        public ViewA(IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator)
        {
            InitializeComponent();
        }
    }
}
