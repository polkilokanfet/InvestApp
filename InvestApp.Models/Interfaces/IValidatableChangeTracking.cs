using System.ComponentModel;

namespace InvestApp.Domain.Interfaces
{
    public interface IValidatableChangeTracking : IRevertibleChangeTracking, INotifyPropertyChanged
    {
        bool IsValid { get; }
    }
}