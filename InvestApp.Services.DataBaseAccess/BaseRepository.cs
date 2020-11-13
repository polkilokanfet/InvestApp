using System;
using System.Collections.Generic;
using System.Linq;
using InvestApp.Domain.Interfaces;
using InvestApp.Domain.Services.DataBaseAccess;
using Microsoft.EntityFrameworkCore;

namespace InvestApp.Services.DataBaseAccess
{
    public class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IBaseEntity
    {
        protected readonly DbContext Context;

        public BaseRepository(DbContext context)
        {
            Context = context;
        }

        //public virtual async Task<List<TEntity>> GetAllAsync()
        //{
        //    Context.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        //    return await GetQuary().ToListAsync();
        //}

        public virtual List<TEntity> GetAll()
        {
            return GetQuary().ToList();
        }

        //public virtual async Task<List<TEntity>> GetAllAsNoTracking()
        //{
        //    Context.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        //    return await GetQuary().AsNoTracking().ToListAsync();
        //}

        public virtual List<TEntity> GetAllAsNoTracking()
        {
            return GetQuary().AsNoTracking().ToList();
        }

        public virtual List<TEntity> Find(Func<TEntity, bool> predicate)
        {
            return GetQuary().AsEnumerable().Where(predicate).ToList();
        }

        public List<TEntity> FindAsNoTracking(Func<TEntity, bool> predicate)
        {
            return GetQuary().AsNoTracking().Where(predicate).ToList();
        }

        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }


        public void Delete(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public TEntity GetById(Guid id)
        {
            return GetQuary().SingleOrDefault(x => x.Id == id);
        }

        //public virtual async Task<TEntity> GetByIdAsync(Guid id)
        //{
        //    Context.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        //    return await GetQuary().SingleOrDefaultAsync(x => x.Id == id);
        //}

        public void Reload(TEntity entity)
        {
            //if (Context.Entry(entity).State == EntityState.Detached)
            //    //if (Context.Set<TEntity>().Local.All(x => x.Id != entity.Id))
            //    Context.Set<TEntity>().Attach(entity);
            //var entry = Context.Entry(entity);
            //entry.Reload();
        }

        protected virtual IQueryable<TEntity> GetQuary()
        {
            return Context.Set<TEntity>().AsQueryable();
        }
    }
}
