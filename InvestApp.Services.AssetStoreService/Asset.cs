using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using InvestApp.Domain.Interfaces;
using InvestApp.Domain.Models;
using InvestApp.Services.TinkoffOpenApiService.Models;

namespace InvestApp.Services.AssetStoreService
{
    public class Asset : IAsset
    {
        public ObservableCollection<Operation> Operations { get; }
        private IEnumerable<Operation> OperationsBuy => Operations.Where(operation => operation.OperationType == ExtendedOperationType.Buy);
        private IEnumerable<Operation> OperationsSell => Operations.Where(operation => operation.OperationType == ExtendedOperationType.Sell);

        public Instrument Instrument { get; }
        public int Shares => OperationsBuy.Sum(operation => operation.Quantity) - OperationsSell.Sum(operation => operation.Quantity);
        public double PricePerShareBuy => GetPricePerShareStart();
        public double PricePerShare { get; }

        public Asset(IEnumerable<Operation> operations, Instrument instrument, double price)
        {
            Operations = new ObservableCollection<Operation>(operations);
            Instrument = instrument;
            PricePerShare = price;
        }

        private double GetPricePerShareStart()
        {
            var buy = OperationsBuy.SelectMany(OperationSingleConverter).OrderBy(operation => operation.Date).ToList();
            var sell = OperationsSell.SelectMany(OperationSingleConverter).OrderBy(operation => operation.Date).ToList();

            var result = buy.ToList();
            foreach (var operation in sell)
            {
                result.Remove(result.First());
            }

            return result.Sum(x => x.Price) / result.Count;
        }

        private IEnumerable<OperationSingle> OperationSingleConverter(Operation operation)
        {
            for (int i = 0; i < operation.Quantity; i++)
            {
                yield return new OperationSingle(operation.Date, (double)operation.Price);
            }
        }

        private class OperationSingle
        {
            public DateTime Date { get; }
            public double Price { get; }

            public OperationSingle(DateTime date, double price)
            {
                Date = date;
                Price = price;
            }
        }
    }
}