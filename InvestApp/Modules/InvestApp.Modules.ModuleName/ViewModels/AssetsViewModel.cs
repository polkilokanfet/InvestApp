using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using InvestApp.Domain.Interfaces;
using InvestApp.Domain.Models;
using InvestApp.Domain.Services;
using InvestApp.Domain.Services.DataBaseAccess;
using InvestApp.Infrastructure.Extansions;
using InvestApp.Services.TinkoffOpenApiService;
using Prism.Commands;
using Prism.Mvvm;

namespace InvestApp.Modules.ModuleName.ViewModels
{
    public class AssetsViewModel : BindableBase
    {
        private readonly IMarketStore _marketStore;
        private readonly IMessageService _messageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ObservableCollection<IAsset> _assets = new ObservableCollection<IAsset>();
        private bool _refreshInProcess = false;
        private IAsset _activeAsset;

        public IEnumerable<IAsset> Assets => _assets;

        public IAsset ActiveAsset
        {
            get => _activeAsset;
            set
            {
                if (Equals(_activeAsset, value)) return;
                _activeAsset = value;
                RaisePropertyChanged();
                ActiveAssetChanged?.Invoke(ActiveAsset);
            }
        }

        public event Action<IAsset> ActiveAssetChanged;

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

        public AssetsViewModel(IMarketStore marketStore, IMessageService messageService, IUnitOfWork unitOfWork)
        {
            _marketStore = marketStore;
            _messageService = messageService;
            _unitOfWork = unitOfWork;

            _assets.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (IAsset asset in args.NewItems.Cast<IAsset>())
                    {
                        asset.InstrumentLastPriceChanged += AssetOnInstrumentLastPriceChanged;
                        asset.TransactionExchangeRateChanged += AssetOnTransactionExchangeRateChanged;
                    }
                }

                if (args.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (IAsset asset in args.OldItems.Cast<IAsset>())
                    {
                        asset.InstrumentLastPriceChanged -= AssetOnInstrumentLastPriceChanged;
                        asset.TransactionExchangeRateChanged -= AssetOnTransactionExchangeRateChanged;
                    }
                }
            };

            _assets.AddRange(_marketStore.GetAssets());

            RefreshCommand = new DelegateCommand(Refresh, () => !RefreshInProcess);

            this.ActiveAssetChanged += asset =>
            {
                _marketStore?.GetCompanyProfileAsync(asset.Instrument.Ticker).Await(
                    companyProfile =>
                    {

                    },
                    e =>
                    {

                    });
            };

            //Refresh();
        }

        private void AssetOnTransactionExchangeRateChanged(Transaction transaction, decimal exchangeRate)
        {
            _unitOfWork.Repository<Transaction>().GetById(transaction.Id).ExchangeRate = exchangeRate;
            _unitOfWork.SaveChanges();
        }

        private void AssetOnInstrumentLastPriceChanged(Instrument instrument, decimal price)
        {
            _unitOfWork.Repository<Instrument>().GetById(instrument.Id).LastPrice = price;
            _unitOfWork.SaveChanges();
        }

        private void Refresh()
        {
            RefreshInProcess = true;

            _ = _marketStore.RefreshTransactionsAsync().Await(
                transactionsNew =>
                {
                    foreach (var transactions in transactionsNew.Where(transaction => transaction.Instrument != null).GroupBy(transaction => transaction.Instrument))
                    {
                        Asset asset = (Asset)Assets.Where(x => x.Instrument != null).SingleOrDefault(x => x.Instrument.Id == transactions.Key.Id);
                        if (asset != null)
                        {
                            asset.Transactions.AddRange(transactions);
                        }
                        else
                        {
                            _assets.Add(new Asset(transactions, _marketStore));
                        }
                    }

                    RefreshAssetsLastPrices();

                    RefreshInProcess = false;
                },
                exception =>
                {
                    _messageService.ShowMessage(exception.Message);
                    RefreshInProcess = false;
                });
        }

        /// <summary>
        /// Обновление цен на активы
        /// </summary>
        /// <returns></returns>
        private void RefreshAssetsLastPrices()
        {
            foreach (IAsset asset in Assets)
            {
                _ = _marketStore.RefreshInstrumentLastPriceAsync(asset.Instrument.Figi).Await(
                    price =>
                    {
                        ((Asset) asset).PricePerShare = price;
                    }, 
                    exception =>
                    {
                        _messageService.ShowMessage(exception.Message);
                    });
            }
        }
    }
}