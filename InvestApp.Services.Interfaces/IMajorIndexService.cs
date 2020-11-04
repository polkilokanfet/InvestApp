using System.Threading.Tasks;
using InvestApp.Models.Models;

namespace InvestApp.Services.Interfaces
{
    public interface IMajorIndexService
    {
        Task<MajorIndex> GetMajorIndex(MajorIndexType indexType);
    }
}