using System.Collections.ObjectModel;
using System.Windows.Input;
using InvestApp.Core.ApplicationCommands;
using Prism.Commands;
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

        public DelegateCommand<INavigationItem> SelectedCommand { get; }

        protected BaseMenuViewModel(IApplicationCommands applicationCommands)
        {
            Items = new ObservableCollection<NavigationItem>();
            
            SelectedCommand = new DelegateCommand<INavigationItem>(
                (navigationItem) =>
                {
                    applicationCommands.NavigateCommand.Execute(navigationItem.NavigationUri);
                });

            GenerateMenu();
        }

        protected abstract void GenerateMenu();
    }
}