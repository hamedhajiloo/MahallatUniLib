using Common;
using Data.Repositories;
using Entities;
using Sepehran.Pooshako.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public interface IBaseService<TEntity> where TEntity : class, IEntity
    {
        void  AddAsync(CancellationToken cancellationToken ,TEntity entity);
        Task<TEntity> FindByIdAsync(CancellationToken cancellationToken, object id);
        Task<TEntity> FindFirstAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> whereCondition = null,
            bool asNoTracking = true, params string[] includes);
        Task<T> FindFirstAsync<T>(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> whereCondition = null, bool asNoTracking = true);
        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken,
            Pagable pagable = null,
            Expression<Func<TEntity, bool>> whereCondition = null,
            bool asNoTracking = true,
            params string[] includes);
        Task<List<T>> GetAllAsync<T>(CancellationToken cancellationToken, Pagable pagable = null,
             Expression<Func<TEntity, bool>> whereCondition = null,
             bool asNoTracking = true);
        void RemoveAsync(CancellationToken cancellationToken, TEntity entity);
        void UpdateAsync(CancellationToken cancellationToken, TEntity entity);
    }
}