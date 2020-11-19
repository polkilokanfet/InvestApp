using System.Linq;
using InvestApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InvestApp.Services.DataBaseAccess.Repositories
{
    public class RepositoryCompanyProfiles : BaseRepository<CompanyProfile>
    {
        public RepositoryCompanyProfiles(DbContext context) : base(context)
        {
        }
        protected override IQueryable<CompanyProfile> GetQuary()
        {
            return Context.Set<CompanyProfile>().AsQueryable()
                .Include(transaction => transaction.FinancialRatios)
                .Include(transaction => transaction.CompanyRaitings);
        }

    }
}