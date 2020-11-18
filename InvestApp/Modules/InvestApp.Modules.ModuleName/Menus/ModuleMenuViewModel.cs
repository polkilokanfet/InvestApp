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
            Items.Add(new NavigationItem("Транзакции", typeof(TransactionsAllView)));

            var a = new NavigationItem("Инструменты", typeof(InstrumentsView));
            Items.Add(a);
        }
    }
}