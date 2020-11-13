using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvestApp.Domain.Interfaces
{
    public interface IAssetStore
    {
        Task<IEnumerable<IAsset>> GetAllAssetsAsync();
    }
}