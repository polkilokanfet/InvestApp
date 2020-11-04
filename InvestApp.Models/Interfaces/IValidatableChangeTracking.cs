using System.ComponentModel;

namespace InvestApp.Models.Interfaces
{
    public interface IValidatableChangeTracking : IRevertibleChangeTracking, INotifyPropertyChanged
    {
        bool IsValid { get; }
    }
}