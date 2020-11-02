using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace InvestApp.Core.Mvvm
{
    public abstract class BaseMenuViewModel : BindableBase
    {
        private ObservableCollection<NavigationItem> _items;
        public ObservableCollection<NavigationItem> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        protected BaseMenuViewModel()
        {
            Items = new ObservableCollection<NavigationItem>();
            GenerateMenu();
        }

        protected abstract void GenerateMenu();
    }
}