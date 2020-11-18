using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using InvestApp.Domain.Models;
using InvestApp.Services.TinkoffOpenApiService;
using Prism.Mvvm;

namespace InvestApp.Modules.ModuleName.ViewModels
{
    public class TransactionViewModel : BindableBase
    {
        private readonly ObservableCollection<Transaction> _transactions = new ObservableCollection<Transaction>();
        public IEnumerable<Transaction> Transactions => _transactions;

        public Transaction ActiveTransaction { get; set; }

        public TransactionViewModel(AssetsViewModel assetsViewModel)
        {
            assetsViewModel.ActiveAssetChanged +=
                asset =>
                {
                    _transactions.Clear();
                    if (asset != null && asset is Asset asset1)
                        _transactions.AddRange(asset1.Transactions.OrderByDescending(transaction => transaction.Date));
                };
        }
    }
}