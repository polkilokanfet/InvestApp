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
            var a = new NavigationItem("AAA", typeof(ViewA));
            Items.Add(a);
            var b = new NavigationItem("BBB", typeof(ViewB));
            Items.Add(b);
        }
    }
}