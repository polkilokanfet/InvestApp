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

        public UnitOfWork(DbContext context)
        {
            _context = context;
            _repositoryInstruments = new RepositoryInstruments(_context);
            _repositoryTransactions = new RepositoryTransactions(_context);
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