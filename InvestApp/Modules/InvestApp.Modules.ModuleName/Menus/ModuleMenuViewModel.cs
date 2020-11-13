using InvestApp.Core.ApplicationCommands;
using InvestApp.Core.Mvvm;
using InvestApp.Modules.ModuleName.Views;

namespace InvestApp.Modules.ModuleName.Menus
{
    public class ModuleMenuViewModel : BaseMenuViewModel
    {
        public ModuleMenuViewModel(IApplicationCommands applicationCommands) : base(applicationCommands)
        {
        }

        protected override void GenerateMenu()
        {
            var b = new NavigationItem("Тинькофф", typeof(TinkoffPortfolioView));
            Items.Add(b);
            var a = new NavigationItem("AAA", typeof(ViewA));
            Items.Add(a);
        }
    }
}