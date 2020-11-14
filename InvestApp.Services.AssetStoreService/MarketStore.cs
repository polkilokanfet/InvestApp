using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvestApp.Domain.Interfaces;
using InvestApp.Domain.Models;
using InvestApp.Services.TinkoffOpenApiService;
using InvestApp.Services.TinkoffOpenApiService.Models;

namespace InvestApp.Services.AssetStoreService
{
    public class MarketStore : IMarketStore
    {
        private readonly TinkoffRepository _tinkoffRepository;

        public MarketStore(TinkoffRepository tinkoffRepository)
        {
            _tinkoffRepository = tinkoffRepository;
        }

        public async Task<IEnumerable<IAsset>> GetAssetsAsync()
        {
            List<MarketInstrument> marketInstruments = await _tinkoffRepository.GetMarketInstruments();
            List<Operation> operations = (await _tinkoffRepository.GetOperationsAllAsync()).Where(x => x.Status == OperationStatus.Done).ToList();

            List<IAsset> result = new List<IAsset>();
            foreach (var operationsGroup in operations.GroupBy(operation => operation.Figi))
            {
                string figi = operationsGroup.Key;
                Instrument instrument = MarketInstrumentConverter(marketInstruments.SingleOrDefault(x => x.Figi == figi));
                decimal price = await _tinkoffRepository.GetPrice(figi);
                Asset asset = new Asset(operationsGroup, instrument, (double)price);
                result.Add(asset);
            }

            return result;
        }

        private Instrument MarketInstrumentConverter(MarketInstrument marketInstrument)
        {
            if (marketInstrument == null)
                return null;

            return new Instrument
            {
                Figi = marketInstrument.Figi,
                Isin = marketInstrument.Isin,
                Ticker = marketInstrument.Ticker,
                Lot = marketInstrument.Lot,
                MinPriceIncrement = marketInstrument.MinPriceIncrement,
                Name = marketInstrument.Name,
                Type = marketInstrument.Type,
                Currency = marketInstrument.Currency
            };
        }
    }
}
