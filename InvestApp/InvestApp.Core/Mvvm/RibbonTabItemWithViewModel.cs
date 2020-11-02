using Infragistics.Windows.Ribbon;
using Prism.Mvvm;

namespace InvestApp.Core.Mvvm
{
    public class RibbonTabItemWithViewModel : RibbonTabItem, IRibbonTabItem
    {
        public BindableBase ViewModel
        {
            get => (BindableBase)DataContext;
            set => DataContext = value;
        }
    }
}