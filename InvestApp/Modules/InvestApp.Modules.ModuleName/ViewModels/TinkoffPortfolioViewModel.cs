using Prism.Mvvm;

namespace InvestApp.Modules.ModuleName.ViewModels
{
    public class TinkoffPortfolioViewModel : BindableBase
    {
        public AssetsViewModel AssetsViewModel { get; }
        public TransactionViewModel TransactionViewModel { get; }

        public TinkoffPortfolioViewModel(AssetsViewModel assetsViewModel, TransactionViewModel transactionViewModel)
        {
            AssetsViewModel = assetsViewModel;
            TransactionViewModel = transactionViewModel;
        }
    }
}
