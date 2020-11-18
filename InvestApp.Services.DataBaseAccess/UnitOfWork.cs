using System.Linq;
using System.Reflection;
using InvestApp.Domain.Interfaces;
using InvestApp.Domain.Models;
using InvestApp.Domain.Services.DataBaseAccess;
using InvestApp.Services.DataBaseAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InvestApp.Services.DataBaseAccess
{
    public partial class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        private readonly IRepository<Instrument> _repositoryInstruments;
        private readonly IRepository<Transaction> _repositoryTransactions;
        private readonly IRepository<CompanyProfile> _repositoryCompanyProfiles;
        private readonly IRepository<Country> _repositoryCountries;
        private readonly IRepository<Exchange> _repositoryExchanges;
        private readonly IRepository<Industry> _repositoryIndustries;
        private readonly IRepository<Sector> _repositorySectors;
        private readonly IRepository<FinancialRatio> _repositoryFinancialRatios;

        public UnitOfWork(DbContext context)
        {
            _context = context;

            _repositoryInstruments = new RepositoryInstruments(context);
            _repositoryTransactions = new RepositoryTransactions(context);
            _repositoryCountries = new RepositoryCountries(context);
            _repositoryExchanges = new BaseRepository<Exchange>(context);
            _repositoryIndustries = new BaseRepository<Industry>(context);
            _repositorySectors = new BaseRepository<Sector>(context);
            _repositoryCompanyProfiles = new BaseRepository<CompanyProfile>(context);
            _repositoryFinancialRatios = new BaseRepository<FinancialRatio>(context);
        }

        public IRepository<T> Repository<T>() where T : class, IBaseEntity
        {
            var repositoryFieldInfo = this.GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Single(x => typeof(IRepository<T>).IsAssignableFrom(x.FieldType));

            return (IRepository<T>)repositoryFieldInfo.GetValue(this);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}