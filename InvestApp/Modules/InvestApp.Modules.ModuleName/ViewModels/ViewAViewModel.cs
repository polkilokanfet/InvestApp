using System.Collections.ObjectModel;
using System.Windows.Input;
using InvestApp.Core.Mvvm;
using InvestApp.Models;
using InvestApp.Models.Models;
using InvestApp.Services.Interfaces;
using Prism.Commands;
using Prism.Regions;

namespace InvestApp.Modules.ModuleName.ViewModels
{
    public class ViewAViewModel : RegionViewModelBase
    {
        private readonly IRepository _repository;
        private readonly IMajorIndexService _majorIndexService;

        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public ObservableCollection<StockListItem> Collection { get; } = new ObservableCollection<StockListItem>();

        public ICommand LoadCommand { get; }

        public ViewAViewModel(IRegionManager regionManager, IRepository repository, IMajorIndexService majorIndexService, IStockPriceService stockPriceService, IStockListService stockListService) : base(regionManager)
        {
            _repository = repository;
            _majorIndexService = majorIndexService;

            LoadCommand = new DelegateCommand(
                async () =>
                {
                    var list = await stockListService.GetStockList();
                    Collection.AddRange(list);
                    //var price = await stockPriceService.GetPrice("VOO");
                    //var indx = await _majorIndexService.GetMajorIndex(MajorIndexType.DowJones);
                    //Collection.Clear();
                    //Collection.AddRange(await _repository.GetOperationsAsync());
                });
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            //do something
        }

    }
}
