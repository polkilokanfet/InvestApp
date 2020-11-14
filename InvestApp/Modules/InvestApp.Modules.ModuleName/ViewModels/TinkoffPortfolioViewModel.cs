using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using InvestApp.Domain.Interfaces;
using InvestApp.Infrastructure.Extansions;
using InvestApp.Services.TinkoffOpenApiService;
using Prism.Commands;

namespace InvestApp.Modules.ModuleName.ViewModels
{
    public class TinkoffPortfolioViewModel : BindableBase
    {
        private readonly IMarketStore _marketStore;
        private readonly ObservableCollection<IAsset> _assets;
        private bool _refreshInProcess = false;

        public IEnumerable<IAsset> Assets => _assets;

        public bool RefreshInProcess
        {
            get => _refreshInProcess;
            set
            {
                if (Equals(_refreshInProcess, value)) return;
                _refreshInProcess = value;
                ((DelegateCommand)RefreshCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand RefreshCommand { get; }

        public TinkoffPortfolioViewModel(IMarketStore marketStore)
        {
            _marketStore = marketStore;

            _assets = new ObservableCollection<IAsset>(_marketStore.GetAssets());

            RefreshCommand = new DelegateCommand(Refresh, () => !RefreshInProcess);

            Refresh();
        }

        private void Refresh()
        {
            RefreshInProcess = true;

            _ = _marketStore.RefreshTransactionsAsync().Await(
                async transactionsNew =>
                {
                    foreach (var transactions in transactionsNew.GroupBy(transaction => transaction.Instrument))
                    {
                        Asset asset = (Asset)Assets.SingleOrDefault(x => x.Instrument.Id == transactions.Key.Id);
                        if (asset != null)
                        {
                            asset.Transactions.AddRange(transactions);
                        }
                        else
                        {
                            _assets.Add(new Asset(transactions));
                        }
                    }

                    await RefreshAssetsLastPrices();

                    RefreshInProcess = false;
                },
                exception =>
                {
                });
        }

        private async Task RefreshAssetsLastPrices()
        {
            foreach (IAsset asset in Assets)
            {
                ((Asset)asset).PricePerShare = await _marketStore.RefreshInstrumentLastPriceAsync(asset.Instrument.Figi);
            }
        }
    }
}
