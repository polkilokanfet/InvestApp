using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvesApp.Services.Tinkoff;
using InvestApp.Domain.Interfaces;
using InvestApp.Domain.Models;
using MarketInstrument = Tinkoff.Trading.OpenApi.Models.MarketInstrument;
using MarketInstrumentDomain = InvestApp.Domain.Models.MarketInstrument;
using Operation = Tinkoff.Trading.OpenApi.Models.Operation;
using OperationStatus = Tinkoff.Trading.OpenApi.Models.OperationStatus;

namespace InvestApp.Services.AssetStoreService
{
    public class AssetStore : IAssetStore
    {
        private readonly TinkoffRepository _tinkoffRepository;

        public AssetStore(TinkoffRepository tinkoffRepository)
        {
            _tinkoffRepository = tinkoffRepository;
        }

        public async Task<IEnumerable<IAsset>> GetAllAssetsAsync()
        {
            List<MarketInstrument> marketInstruments = await _tinkoffRepository.GetMarketInstruments();
            List<Operation> operations = (await _tinkoffRepository.GetOperationsAllAsync()).Where(x => x.Status == OperationStatus.Done).ToList();

            List<IAsset> result = new List<IAsset>();
            foreach (var operationsGroup in operations.GroupBy(operation => operation.Figi))
            {
                string figi = operationsGroup.Key;
                MarketInstrumentDomain marketInstrument = MarketInstrumentConverter(marketInstruments.SingleOrDefault(x => x.Figi == figi));
                decimal price = await _tinkoffRepository.GetPrice(figi);
                Asset asset = new Asset(operationsGroup, marketInstrument, (double)price);
                result.Add(asset);
            }

            return result;
        }

        private MarketInstrumentDomain MarketInstrumentConverter(MarketInstrument marketInstrument)
        {
            if (marketInstrument == null)
                return null;

            return new MarketInstrumentDomain()
            {
                Figi = marketInstrument.Figi,
                Isin = marketInstrument.Isin,
                Ticker = marketInstrument.Ticker,
                Lot = marketInstrument.Lot,
                MinPriceIncrement = marketInstrument.MinPriceIncrement,
                Name = marketInstrument.Name,
                Type = (InstrumentType)((int)marketInstrument.Type),
                Currency = (Currency)((int)marketInstrument.Currency)
            };
        }
    }
}
