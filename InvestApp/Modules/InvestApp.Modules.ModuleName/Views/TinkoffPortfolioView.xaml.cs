using InvestApp.Core.Attributes;
using InvestApp.Modules.ModuleName.Tabs;

namespace InvestApp.Modules.ModuleName.Views
{
    [RibbonTab(typeof(TinkoffPortfolioViewTab))]
    public partial class TinkoffPortfolioView
    {
        public TinkoffPortfolioView()
        {
            InitializeComponent();
        }
    }
}
