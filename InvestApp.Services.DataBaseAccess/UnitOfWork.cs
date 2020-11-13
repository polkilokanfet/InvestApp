using System.Linq;
using System.Reflection;
using InvestApp.Domain.Interfaces;
using InvestApp.Domain.Services.DataBaseAccess;
using Microsoft.EntityFrameworkCore;

namespace InvestApp.Services.DataBaseAccess
{
    public partial class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        public UnitOfWork(DbContext context)
        {
            _context = context;
            InitializeRepositories();
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