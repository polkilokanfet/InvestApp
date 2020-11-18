using System.Collections.Generic;
using System.Collections.ObjectModel;
using InvestApp.Core.Mvvm;
using InvestApp.Domain.Models;
using InvestApp.Domain.Services.DataBaseAccess;
using Prism.Regions;

namespace InvestApp.Modules.ModuleName.ViewModels
{
    public class InstrumentsViewModel : RegionViewModelBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ObservableCollection<Instrument> Instruments { get; }

        public InstrumentsViewModel(IRegionManager regionManager, IUnitOfWork unitOfWork) : base(regionManager)
        {
            _unitOfWork = unitOfWork;
            List<Instrument> instruments = unitOfWork.Repository<Instrument>().GetAll();
            Instruments = new ObservableCollection<Instrument>(instruments);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            //do something
        }

    }
}
