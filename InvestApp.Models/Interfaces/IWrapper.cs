using System.ComponentModel.DataAnnotations;

namespace InvestApp.Domain.Interfaces
{
    public interface IWrapper<out TModel> : IValidatableChangeTracking, IValidatableObject
        where TModel : class, IBaseEntity
    {
        TModel Model { get; }
        void Refresh();
    }
}