using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using InvestApp.Domain.Interfaces;
using InvestApp.Domain.Models;
using InvestApp.Infrastructure.Extansions;
using InvestApp.Services.TinkoffOpenApiService.Annotations;

namespace InvestApp.Services.TinkoffOpenApiService
{
    //public class AssetCurrency : IAsset
    //{
    //    private readonly Currency _currency;
    //    private Instrument _instrument;
    //    private int _shares;
    //    private decimal _pricePerShareBuy;
    //    private decimal _pricePerShare;

    //    private readonly ObservableCollection<Transaction> _transactions;
    //    public IEnumerable<Transaction> Transactions => _transactions;

    //    public Instrument Instrument => _instrument;

    //    public int Shares
    //    {
    //        get
    //        {
    //            int result = 0;

    //            foreach (var transaction in Transactions.Where(x => x.Status == OperationStatus.Done))
    //            {
    //                if (transaction.Currency != _currency)
    //            }

    //            return result;
    //        }
    //    }

    //    public decimal PricePerShareBuy => _pricePerShareBuy;

    //    public decimal PricePerShare => _pricePerShare;

    //    public event Action<Instrument, decimal> InstrumentLastPriceChanged;
    //    public event Action<Transaction, decimal> TransactionExchangeRateChanged;

    //    public AssetCurrency(IEnumerable<Transaction> transactions, Currency currency)
    //    {
    //        _currency = currency;
    //        _transactions = new ObservableCollection<Transaction>(transactions);
    //    }
    //}

    public class Asset : IAsset, INotifyPropertyChanged
    {
        #region Fields

        private readonly IMarketStore _marketStore;
        private decimal _pricePerShare;
        private decimal _pricePerShareBuy;
        private decimal _profitLoss;
        private decimal? _profitLossDone;

        #endregion

        #region Transactions

        /// <summary>
        /// Транзакции по активу
        /// </summary>
        public ObservableCollection<Transaction> Transactions { get; }

        /// <summary>
        /// Совершенные транзакции
        /// </summary>
        private IEnumerable<Transaction> TransactionsDone => Transactions.Where(transaction => transaction.Status == OperationStatus.Done);

        /// <summary>
        /// Транзакции покупки
        /// </summary>
        private IEnumerable<Transaction> TransactionsBuy => TransactionsDone
            .Where(transaction => transaction.OperationType == ExtendedOperationType.Buy || transaction.OperationType == ExtendedOperationType.BuyCard);

        /// <summary>
        /// Транзакции продажи
        /// </summary>
        private IEnumerable<Transaction> TransactionsSell => TransactionsDone
            .Where(operation => operation.OperationType == ExtendedOperationType.Sell);

        #endregion

        public Instrument Instrument { get; }

        public int Shares
        {
            get
            {
                //если бумага погашена (облигации)
                if (TransactionsDone.Any(transaction => transaction.OperationType == ExtendedOperationType.Repayment))
                {
                    return 0;
                }

                return TransactionsBuy.Sum(transaction => transaction.Quantity) -
                       TransactionsSell.Sum(transaction => transaction.Quantity);
            }
        }

        #region Prices

        public decimal PricePerShareBuy
        {
            get => _pricePerShareBuy;
            private set
            {
                if (Equals(_pricePerShareBuy, value)) return;
                _pricePerShareBuy = value;
                OnPropertyChanged();
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
                OnPropertyChanged(nameof(ProfitLoss));
                OnPropertyChanged(nameof(ProfitLossDone));
                OnPropertyChanged(nameof(ProfitLossVirtual));
            }
        }

        #endregion

        #region ProfitLoss

        /// <summary>
        /// P/L
        /// </summary>
        public decimal ProfitLoss
        {
            get => _profitLoss;
            set
            {
                if (Equals(_profitLoss, value)) return;
                _profitLoss = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<Transaction> TransactionsWithPayment => TransactionsDone.Where(transaction => transaction.Payment != 0);
        private int CurrenciesAmount => TransactionsWithPayment.Select(transaction => transaction.Currency).Distinct().Count();

        private async Task<decimal> GetProfitLoss()
        {
            //Приведение транзакции к платежам в валюте инструмента
            List<decimal> payments = new List<decimal>();
            foreach (var transaction in TransactionsWithPayment)
                payments.Add(await TransactionToPayment(transaction));

            //добавление виртуальной продажи актива по текущей рыночной цене
            payments.Add(Shares * PricePerShare);

            return payments.Sum();
        }

        /// <summary>
        /// Приведение транзакции к платежу в валюте инструмента
        /// </summary>
        /// <returns></returns>
        private async Task<decimal> TransactionToPayment(Transaction transaction)
        {
            //если валюты совпадают
            if (transaction.Currency == Instrument.Currency)
                return transaction.Payment;

            //если сохранен курс конвертации
            if (transaction.ExchangeRate.HasValue)
                return transaction.Payment * transaction.ExchangeRate.Value;

            //получение курса конвертации
            decimal exchangeRateInstrument = await _marketStore.GetRubExchangeRate(Instrument.Currency, transaction.Date);
            decimal exchangeRateTransaction = await _marketStore.GetRubExchangeRate(transaction.Currency, transaction.Date);
            decimal exchangeRate = exchangeRateTransaction / exchangeRateInstrument;

            //сохранение курса конвертации
            if (!Equals(transaction.ExchangeRate, exchangeRate))
                TransactionExchangeRateChanged?.Invoke(transaction, exchangeRate);

            return transaction.Payment * exchangeRate;
        }

        /// <summary>
        /// P/L реализованный
        /// </summary>
        public decimal? ProfitLossDone
        {
            get => _profitLossDone;
            set
            {
                if (Equals(_profitLossDone, value)) return;
                _profitLossDone = value;
                OnPropertyChanged();
            }
        }

        private async Task<decimal> GetProfitLossDone()
        {
            if (Shares == 0)
            {
                return TransactionsDone.Sum(transaction => transaction.Payment);
            }

            List<Transaction> transactionsWithQuant = TransactionsDone.Where(transaction => transaction.Quantity > 0).ToList();

            List<SingleLotTransactionPayment> profitTransactions =
                (await SingleLotPaymentConverter(transactionsWithQuant.Where(transaction => transaction.Payment > 0)))
                    .OrderBy(singleLotTransaction => singleLotTransaction.Date)
                    .ToList();

            List<SingleLotTransactionPayment> lossTransactions =
                (await SingleLotPaymentConverter(transactionsWithQuant.Where(transaction => transaction.Payment < 0)))
                    .OrderBy(singleLotTransaction => singleLotTransaction.Date)
                    .Take(profitTransactions.Count)
                    .ToList();

            //приводим комиссии к валюте бумаги
            List<decimal> commissions = new List<decimal>();
            foreach (Transaction transaction1 in TransactionsDone.Where(transaction => transaction.Quantity == 0))
            {
                commissions.Add(await TransactionToPayment(transaction1));
            }

            return profitTransactions.Sum(transaction => transaction.Lot * transaction.PaymentUnit) +
                   lossTransactions.Sum(transaction => transaction.Lot * transaction.PaymentUnit) +
                   commissions.Sum();
        }

        /// <summary>
        /// P/L не реализованный
        /// </summary>
        public decimal? ProfitLossVirtual => ProfitLoss - ProfitLossDone;

        #endregion

        public string Exceptions { get; private set; }

        #region Events

        public event Action<Instrument, decimal> InstrumentLastPriceChanged;
        public event Action<Transaction, decimal> TransactionExchangeRateChanged;

        #endregion

        #region ctor

        public Asset(IEnumerable<Transaction> transactions, IMarketStore marketStore)
        {
            if (transactions == null)
                throw new ArgumentNullException(nameof(transactions));

            if (!transactions.Any())
                throw new ArgumentException(nameof(transactions));

            _marketStore = marketStore;

            Transactions = new ObservableCollection<Transaction>(transactions);
            Transactions.CollectionChanged += (sender, args) =>
            {
                OnPropertyChanged(nameof(Shares));
                OnPropertyChanged(nameof(PricePerShareBuy));
                Refresh();
            };

            Instrument = Transactions.First().Instrument;
            _pricePerShare = Instrument.LastPrice;
            Refresh();
        }

        #endregion

        private void AddException(string what, string message)
        {
            Exceptions = $"Ошибка обновления {what} от {DateTime.Now.ToShortTimeString()}: {message}; {Exceptions}";
            OnPropertyChanged(nameof(Exceptions));
        }

        public void Refresh()
        {
            PricePerShareBuy = GetPricePerShareBuy();
            _marketStore?.GetPrice(Instrument.Figi).Await(
                price =>
                {
                    if (!Equals(Instrument.LastPrice, price))
                    {
                        PricePerShare = price;
                        Instrument.LastPrice = price;
                        this.InstrumentLastPriceChanged?.Invoke(Instrument, price);
                    }
                },
                exception =>
                {
                    AddException("цены", exception.Message);
                });

            GetProfitLoss()?.Await(
                profitLoss =>
                {
                    this.ProfitLoss = profitLoss;
                    OnPropertyChanged(nameof(ProfitLossVirtual));
                },
                exception =>
                {
                    AddException("P/L", exception.Message);
                });

            GetProfitLossDone()?.Await(
                profitLossDone =>
                {
                    this.ProfitLossDone = profitLossDone;
                    OnPropertyChanged(nameof(ProfitLossVirtual));
                },
                exception =>
                {
                    AddException("P/L done", exception.Message);
                });
        }

        /// <summary>
        /// Вычисление цены покупки актива
        /// </summary>
        /// <returns></returns>
        private decimal GetPricePerShareBuy()
        {
            //общее количество проданных лотов
            var sellLotsAmount = TransactionsSell.Sum(transaction => transaction.Quantity) / Instrument.Lot;

            //купленные лоты
            List<SingleLotTransaction> buy = SingleLotPriceConverter(TransactionsBuy)
                .OrderBy(singleLotTransaction => singleLotTransaction.Date)
                .Skip(sellLotsAmount).ToList();

            return buy.Any() ? buy.Sum(singleLotTransaction => singleLotTransaction.Price) / buy.Count : 0;
        }

        #region SingleLotTransaction

        private IEnumerable<SingleLotTransaction> SingleLotPriceConverter(IEnumerable<Transaction> transactions)
        {
            foreach (Transaction transaction in transactions.Where(transaction => transaction.Quantity > 0))
            {
                int lot = transaction.Instrument.Lot;
                for (int i = 0; i < transaction.Quantity / lot; i++)
                {
                    yield return new SingleLotTransaction(transaction.Date, transaction.Price, lot);
                }
            }
        }

        private readonly struct SingleLotTransaction
        {
            public DateTime Date { get; }
            public decimal Price { get; }
            public int Lot { get; }

            public SingleLotTransaction(DateTime date, decimal price, int lot)
            {
                Date = date;
                Price = price;
                Lot = lot;
            }
        }

        private async Task<IEnumerable<SingleLotTransactionPayment>> SingleLotPaymentConverter(IEnumerable<Transaction> transactions)
        {
            List<SingleLotTransactionPayment> result = new List<SingleLotTransactionPayment>();
            foreach (Transaction transaction in transactions.Where(transaction => transaction.Quantity > 0))
            {
                int lot = transaction.Instrument.Lot;
                for (int i = 0; i < transaction.Quantity / lot; i++)
                {
                    decimal paymentUnit = await TransactionToPayment(transaction) / transaction.Quantity;
                    result.Add(new SingleLotTransactionPayment(transaction.Date, lot, paymentUnit));
                }
            }

            return result;
        }

        private readonly struct SingleLotTransactionPayment
        {
            public DateTime Date { get; }
            public int Lot { get; }
            /// <summary>
            /// Платеж за единицу
            /// </summary>
            public decimal PaymentUnit { get; }

            public SingleLotTransactionPayment(DateTime date, int lot, decimal paymentUnit)
            {
                Date = date;
                Lot = lot;
                PaymentUnit = paymentUnit;
            }
        }

        #endregion

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