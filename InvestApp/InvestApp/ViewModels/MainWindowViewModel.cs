using System;
using InvestApp.Core;
using InvestApp.Core.ApplicationCommands;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace InvestApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "InvestApp";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public MainWindowViewModel(IRegionManager regionManager, IApplicationCommands applicationCommands)
        {
            applicationCommands.NavigateCommand.RegisterCommand(new DelegateCommand<Uri>(
                (uri) =>
                {
                    if (uri != null)
                        regionManager.RequestNavigate(RegionNames.ContentRegion, uri);
                }));
        }
    }
}
