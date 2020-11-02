using Prism.Mvvm;

namespace InvestApp.Core.Mvvm
{
    public interface IRibbonTabItem
    {
        BindableBase ViewModel { get; set; }
        bool IsSelected { get; set; }
    }
}