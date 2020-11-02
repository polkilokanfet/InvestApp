using Prism.Commands;

namespace InvestApp.Core.ApplicationCommands
{
    public interface IApplicationCommands
    {
        CompositeCommand NavigateCommand { get; }
    }
}