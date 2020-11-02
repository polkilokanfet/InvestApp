using System.Collections.Generic;
using System.Threading.Tasks;
using InvestApp.Models;

namespace InvestApp.Services.Interfaces
{
    public interface IRepository
    {
        Task<List<Operation>> GetOperationsAsync();
    }
}
