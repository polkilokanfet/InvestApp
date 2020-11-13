using System;
using InvestApp.Domain.Interfaces;

namespace InvestApp.Domain.Services.DataBaseAccess
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Вернуть репозиторий
        /// </summary>
        /// <typeparam name="T">Тип сущности из репозитория</typeparam>
        /// <returns> Репозиторий </returns>
        IRepository<T> Repository<T>() where T : class, IBaseEntity;

        ///// <summary>
        ///// Сохранить все изменения
        ///// </summary>
        ///// <returns></returns>
        //Task<int> SaveChangesAsync();

        /// <summary>
        /// Сохранить все изменения
        /// </summary>
        /// <returns></returns>
        void SaveChanges();
    }
}