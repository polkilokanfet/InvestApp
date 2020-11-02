using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using InvestApp.Core.Annotations;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace InvestApp.Core.Mvvm
{
    public class ViewBase : UserControl, IViewBase, INavigationAware, INotifyPropertyChanged
    {
        public IRegionManager RegionManager { get; }
        public IEventAggregator EventAggregator { get; }
        public IList<IRibbonTabItem> RibbonTabs { get; }

        public BindableBase ViewModel
        {
            get => (BindableBase)DataContext;
            set => DataContext = value;
        }

        public ViewBase()
        {
        }

        public ViewBase(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            RegionManager = regionManager;
            EventAggregator = eventAggregator;

            RibbonTabs = new List<IRibbonTabItem>();
        }

        #region INavigationAware
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            //добавляем RibbonTabs
            if (RegionManager != null)
            {
                IRegion tabRegion = RegionManager.Regions[RegionNames.RibbonTabRegion];
                RibbonTabs.ToList().ForEach(tab => tabRegion.Add(tab));
            }
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //удаляем RibbonTabs
            if (RegionManager != null)
            {
                IRegion tabRegion = RegionManager.Regions[RegionNames.RibbonTabRegion];
                RibbonTabs.ToList().ForEach(tab => tabRegion.Remove(tab));
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
