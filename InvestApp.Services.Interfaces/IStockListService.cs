using System.Collections.Generic;
using System.Threading.Tasks;
using InvestApp.Models.Models;

namespace InvestApp.Services.Interfaces
{
    public interface IStockListService
    {
        Task<List<StockListItem>> GetStockList();
    }
}