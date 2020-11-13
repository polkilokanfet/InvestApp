using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InvestApp.Domain.Interfaces;
using InvestApp.Domain.Models;

namespace InvestApp.Domain.Services.DataBaseAccess
{
    public interface IRepository<TEntity>
        where TEntity : class, IBaseEntity
    {
        //Task<List<TEntity>> GetAllAsync();
        List<TEntity> GetAll();

        List<TEntity> GetAllAsNoTracking();
        //Task<List<TEntity>> GetAllAsNoTrackingAsync();

        TEntity GetById(Guid id);
        //Task<TEntity> GetByIdAsync(Guid id);

        List<TEntity> Find(Func<TEntity, bool> predicate);
        List<TEntity> FindAsNoTracking(Func<TEntity, bool> predicate);

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);

        void Reload(TEntity entity);
    }
}
