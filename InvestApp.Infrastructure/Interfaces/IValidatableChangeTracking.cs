using System.ComponentModel;

namespace InvestApp.Infrastructure.Interfaces
{
    public interface IValidatableChangeTracking : IRevertibleChangeTracking, INotifyPropertyChanged
    {
        bool IsValid { get; }
    }
}