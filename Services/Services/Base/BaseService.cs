using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using System.Linq.Dynamic;
using Entities;
using Data.Repositories;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Common;

namespace Services
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, IEntity
    {
        private readonly IRepository<TEntity> _repository;

        public BaseService(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public virtual async void AddAsync(CancellationToken cancellationToken, TEntity entity)
        {
            await _repository.AddAsync(entity, cancellationToken);
        }

        public virtual async Task<TEntity> FindByIdAsync(CancellationToken cancellationToken, object id)
        {
            return await _repository.GetByIdAsync(cancellationToken, id);
        }
        public virtual async Task<TEntity> FindFirstAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> whereCondition = null,
            bool asNoTracking = false, params string[] includes)
        {
            var query = _repository.Table;

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (whereCondition != null)
                return await query.FirstOrDefaultAsync(whereCondition);
            else
                return await query.FirstOrDefaultAsync(cancellationToken);
        }
        public virtual async Task<T> FindFirstAsync<T>(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> whereCondition = null, bool asNoTracking = true)
        {
            var query = _repository.Table;

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            if (whereCondition != null)
                return await query.Where(whereCondition)
                    .ProjectTo<T>().FirstOrDefaultAsync(cancellationToken);

            else
                return await query
                    .ProjectTo<T>().FirstOrDefaultAsync(cancellationToken);
        }
        public virtual async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken,
            Pagable pagable = null,
            Expression<Func<TEntity, bool>> whereCondition = null,
            bool asNoTracking = true,
            params string[] includes)
        {
            var query = _repository.Table;

            if (includes != null)
                foreach (var item in includes)
                    query = query.Include(item);

            if (whereCondition != null)
                query = query.Where(whereCondition);

            if (asNoTracking)
                query = query.AsNoTracking();

            if (pagable != null)
            {
                if (pagable.Order != null)
                {
                    foreach (var item in pagable.Order)
                    {
                        if (!string.IsNullOrWhiteSpace(item))
                        {
                            query = query.OrderBy(item);
                        }
                    }
                }

                var count = query.Count();

                var pager = pagable.PageSize != null
                      ? new Pager(count, pagable.Page, pagable.PageSize.Value)
                      : null;

                query = query
                       .Skip((pager.CurrentPage - 1) * pager.PageSize)
                       .Take(pager.PageSize)
                       .AsQueryable();
            }

            return await query.ToListAsync(cancellationToken);
        }

        public virtual async Task<List<T>> GetAllAsync<T>(CancellationToken cancellationToken, Pagable pagable = null,
            Expression<Func<TEntity, bool>> whereCondition = null,
            bool asNoTracking = true)
        {

            var query = _repository.Table;

            if (whereCondition != null)
                query = query.Where(whereCondition);

            if (asNoTracking)
                query = query.AsNoTracking();

            if (pagable != null)
            {
                if (pagable.Order != null)
                {
                    foreach (var item in pagable.Order)
                    {
                        if (!string.IsNullOrWhiteSpace(item))
                        {
                            query = query.OrderBy(item);
                        }
                    }
                }

                var count = query.Count();

                var pager = pagable.PageSize != null
                      ? new Pager(count, pagable.Page, pagable.PageSize.Value)
                      : null;

                query = query
                       .Skip((pager.CurrentPage - 1) * pager.PageSize)
                       .Take(pager.PageSize)
                       .AsQueryable();
            }
            var a = query.ToList();

            return await query.ProjectTo<T>().ToListAsync(cancellationToken);
        }

        public async void RemoveAsync(CancellationToken cancellationToken, TEntity entity)
        {
            await _repository.DeleteAsync(entity, cancellationToken);
        }
        public async void UpdateAsync(CancellationToken cancellationToken, TEntity entity)
        {
            await _repository.UpdateAsync(entity, cancellationToken);
        }
    }
}
