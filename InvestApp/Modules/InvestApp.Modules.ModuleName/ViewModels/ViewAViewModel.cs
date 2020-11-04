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

        public ObservableCollection<Operation> Collection { get; } = new ObservableCollection<Operation>();

        public ICommand LoadCommand { get; }

        public ViewAViewModel(IRegionManager regionManager, IRepository repository, IMajorIndexService majorIndexService, IStockPriceService stockPriceService) : base(regionManager)
        {
            _repository = repository;
            _majorIndexService = majorIndexService;

            LoadCommand = new DelegateCommand(
                async () =>
                {
                    var price = await stockPriceService.GetPrice("AAPL");
                    var indx = await _majorIndexService.GetMajorIndex(MajorIndexType.DowJones);
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
