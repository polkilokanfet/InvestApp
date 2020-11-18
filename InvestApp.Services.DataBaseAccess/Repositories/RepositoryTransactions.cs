using System.Linq;
using InvestApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InvestApp.Services.DataBaseAccess.Repositories
{
    public class RepositoryTransactions : BaseRepository<Transaction>
    {
        public RepositoryTransactions(DbContext context) : base(context)
        {
        }

        protected override IQueryable<Transaction> GetQuary()
        {
            return Context.Set<Transaction>().AsQueryable()
                .Include(transaction => transaction.Instrument)
                .Include(transaction => transaction.Commission);
        }
    }
}