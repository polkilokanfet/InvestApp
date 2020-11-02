using Prism.Commands;

namespace InvestApp.Core.ApplicationCommands
{
    public class ApplicationCommands : IApplicationCommands
    {
        public CompositeCommand NavigateCommand { get; } = new CompositeCommand();
    }
}