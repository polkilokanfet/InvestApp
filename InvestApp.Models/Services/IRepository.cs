using System.Collections.Generic;
using System.Threading.Tasks;
using InvestApp.Domain.Models;

namespace InvestApp.Domain.Services
{
    public interface IRepository
    {
        Task<List<Operation>> GetOperationsAsync();
    }
}
