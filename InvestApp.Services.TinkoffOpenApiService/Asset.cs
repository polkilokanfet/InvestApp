using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using InvestApp.Domain.Interfaces;
using InvestApp.Domain.Models;
using InvestApp.Services.TinkoffOpenApiService.Annotations;

namespace InvestApp.Services.TinkoffOpenApiService
{
    public class Asset : IAsset, INotifyPropertyChanged
    {
        private decimal _pricePerShare;

        public ObservableCollection<Transaction> Transactions { get; }
        private IEnumerable<Transaction> TransactionsBuy => Transactions.Where(operation => operation.OperationType == ExtendedOperationType.Buy);
        private IEnumerable<Transaction> TransactionsSell => Transactions.Where(operation => operation.OperationType == ExtendedOperationType.Sell);

        public Instrument Instrument => Transactions.First().Instrument;
        public int Shares => TransactionsBuy.Sum(operation => operation.Quantity) - TransactionsSell.Sum(operation => operation.Quantity);

        public decimal PricePerShareBuy
        {
            get
            {
                if (Shares == 0) return 0;

                var buy = SingleConverter(TransactionsBuy).OrderBy(operation => operation.Date).ToList();
                var sell = SingleConverter(TransactionsSell).OrderBy(operation => operation.Date).ToList();

                var result = buy.ToList();
                foreach (var operation in sell)
                {
                    result.Remove(result.First());
                }

                return result.Sum(x => x.Price) / result.Count;
            }
        }

        public decimal PricePerShare
        {
            get => _pricePerShare;
            set
            {
                if (Equals(_pricePerShare, value)) return;
                _pricePerShare = value;
                OnPropertyChanged();
            }
        }

        public Asset(IEnumerable<Transaction> transactions)
        {
            if (transactions == null)
                throw new ArgumentNullException(nameof(transactions));

            if (!transactions.Any())
                throw new ArgumentException(nameof(transactions));

            Transactions = new ObservableCollection<Transaction>(transactions);
            Transactions.CollectionChanged += (sender, args) =>
            {
                OnPropertyChanged(nameof(Shares));
                OnPropertyChanged(nameof(PricePerShareBuy));
            };

            _pricePerShare = Transactions.First().Instrument.LastPrice;


        }

        private IEnumerable<SingleTransaction> SingleConverter(IEnumerable<Transaction> transactions)
        {
            foreach (Transaction transaction in transactions)
            {
                for (int i = 0; i < transaction.Quantity; i++)
                {
                    yield return new SingleTransaction(transaction.Date, transaction.Price.Value);
                }
            }
        }

        private class SingleTransaction
        {
            public DateTime Date { get; }
            public decimal Price { get; }

            public SingleTransaction(DateTime date, decimal price)
            {
                Date = date;
                Price = price;
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}