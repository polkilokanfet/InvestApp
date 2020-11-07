using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using InvestApp.Domain.Interfaces;

namespace InvestApp.Domain.Wrappers.Base.TrackingCollections
{
    public interface IValidatableChangeTrackingCollection<TCollectionItem> : IList<TCollectionItem>, IValidatableChangeTracking, INotifyCollectionChanged
        where TCollectionItem : IValidatableChangeTracking
    {
        ReadOnlyObservableCollection<TCollectionItem> AddedItems { get; }
        ReadOnlyObservableCollection<TCollectionItem> ModifiedItems { get; }
        ReadOnlyObservableCollection<TCollectionItem> RemovedItems { get; }
    }
}