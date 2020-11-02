using System.Collections.Generic;
using Prism.Mvvm;

namespace InvestApp.Core.Mvvm
{
    public interface IViewBase
    {
        BindableBase ViewModel { get; set; }
        IList<IRibbonTabItem> RibbonTabs { get; }
    }
}