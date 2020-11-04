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

        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public ObservableCollection<Operation> Collection { get; } = new ObservableCollection<Operation>();

        public ICommand LoadCommand { get; }

        public ViewAViewModel(IRegionManager regionManager, IRepository repository) : base(regionManager)
        {
            _repository = repository;

            LoadCommand = new DelegateCommand(
                async () =>
                {
                    Collection.Clear();
                    Collection.AddRange(await _repository.GetOperationsAsync());
                });
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            //do something
        }

    }
}
