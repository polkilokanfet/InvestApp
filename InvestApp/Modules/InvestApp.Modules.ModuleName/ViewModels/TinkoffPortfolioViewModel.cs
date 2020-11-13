using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using InvesApp.Services.Tinkoff;
using InvestApp.Domain.Interfaces;
using InvestApp.Infrastructure.Extansions;
using Prism.Commands;
using Tinkoff.Trading.OpenApi.Models;
using Position = Tinkoff.Trading.OpenApi.Models.Portfolio.Position;

namespace InvestApp.Modules.ModuleName.ViewModels
{
    public class TinkoffPortfolioViewModel : BindableBase
    {
        private readonly TinkoffRepository _tinkoffRepository;
        private readonly IAssetStore _assetStore;
        private readonly ObservableCollection<Position> _positions = new ObservableCollection<Position>();
        private readonly ObservableCollection<Operation> _operations = new ObservableCollection<Operation>();
        private readonly ObservableCollection<IAsset> _assets = new ObservableCollection<IAsset>();
        
        public IEnumerable<Position> Positions => _positions;
        public IEnumerable<Operation> Operations => _operations;
        public IEnumerable<IAsset> Assets => _assets;

        public Position SelectedPosition { get; set; }

        public ICommand SellPositionCommand { get; }

        public TinkoffPortfolioViewModel(TinkoffRepository tinkoffRepository, IAssetStore assetStore)
        {
            _tinkoffRepository = tinkoffRepository;
            _assetStore = assetStore;
            Load();

            SellPositionCommand = new DelegateCommand(
                async () =>
                {
                    await _tinkoffRepository.SellPositionAsync(SelectedPosition.Figi, SelectedPosition.Lots);
                });
        }

        private void Load()
        {
            _ = _tinkoffRepository.GetPortfolioAsync().Await(
                portfolio =>
                {
                    _positions.Clear();
                    _positions.AddRange(portfolio.Positions);
                });

            _ = _tinkoffRepository.GetOperationsAllAsync().Await(
                operations =>
                {
                    _operations.Clear();
                    _operations.AddRange(operations);
                });

            _ = _assetStore.GetAllAssetsAsync().Await(
                assets =>
                {
                    _assets.Clear();
                    _assets.AddRange(assets);
                },
                exception =>
                {
                });
        }
    }
}
