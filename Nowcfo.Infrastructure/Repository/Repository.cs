using Microsoft.EntityFrameworkCore;
using Nowcfo.Application.Interfaces;
using Nowcfo.Application.Internals;
using Nowcfo.Domain.Specifications.Base;
using Nowcfo.Infrastructure.Persistance.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Nowcfo.Infrastructure.Persistance.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        #region Private fields

        private readonly ApplicationDbContext _context;

        #endregion

        #region Constructor

        public Repository(ApplicationDbContext context) => _context = context;

        #endregion

        #region Private methods

        private DbSet<T> DbSet() => _context.Set<T>();

        #endregion

        #region IInternalRepository

        #region Add

        void IInternalRepository<T>.Add(T entity)
        {
            if (entity == null) throw new Exception("entities cannot be null");
            _context.Add(entity);
        }

        void IInternalRepository<T>.Add(List<T> entities)
        {
            if (entities == null) throw new Exception("entities cannot be null");
            _context.AddRange(entities);
        }

        void IInternalRepository<T>.Add(IEnumerable<T> entities)
        {
            if (entities == null) throw new Exception("entities cannot be null");
            _context.AddRange(entities);
        }

        void IInternalRepository<T>.Add(IQueryable<T> entities)
        {
            if (entities == null) throw new Exception("entities cannot be null");
            _context.AddRange(entities);
        }

        #endregion

        #region Update

        void IInternalRepository<T>.Update(T entity)
        {
            if (entity == null) throw new Exception("entities cannot be null");
            _context.Update(entity);
        }

        void IInternalRepository<T>.Update(List<T> entities)
        {
            if (entities == null) throw new Exception("entities cannot be null");
            _context.UpdateRange(entities);
        }

        void IInternalRepository<T>.Update(IEnumerable<T> entities)
        {
            if (entities == null) throw new Exception("entities cannot be null");
            _context.UpdateRange(entities);
        }

        void IInternalRepository<T>.Update(IQueryable<T> entities)
        {
            if (entities == null) throw new Exception("entities cannot be null");
            _context.UpdateRange(entities);
        }

        #endregion

        #region Remove

        void IInternalRepository<T>.Remove(T entity)
        {
            if (entity == null) throw new Exception("entity cannot be null");
            _context.Remove(entity);
        }

        void IInternalRepository<T>.Remove(List<T> entities)
        {
            if (entities == null) throw new Exception("entities cannot be null");
            _context.RemoveRange(entities);
        }

        void IInternalRepository<T>.Remove(IEnumerable<T> entities)
        {
            if (entities == null) throw new Exception("entities cannot be null");
            _context.RemoveRange(entities);
        }

        void IInternalRepository<T>.Remove(IQueryable<T> entities)
        {
            if (entities == null) throw new Exception("entities cannot be null");
            _context.RemoveRange(entities);
        }

        void IInternalRepository<T>.Remove(int id)
        {
            var entityToRemove = DbSet().Find(id);
            if (entityToRemove == null) return;
            _context.Remove(entityToRemove);
        }

        void IInternalRepository<T>.Remove(Expression<Func<T, bool>> predicate)
        {
            var entitiesToRemove = DbSet().Where(predicate);
            if (!entitiesToRemove.Any()) return;
            _context.RemoveRange(entitiesToRemove);
        }

        #endregion

        #endregion

        #region IAsyncRepository

        async Task IInternalRepository<T>.RemoveAsync(Expression<Func<T, bool>> predicate)
        {
            var entityToRemove = await DbSet().FirstOrDefaultAsync(predicate);
            if (entityToRemove == null) return;
            _context.RemoveRange(entityToRemove);
        }

        async Task IInternalRepository<T>.RemoveAsync(int id)
        {
            var entityToRemove = await DbSet().FindAsync(id);
            if (entityToRemove == null) return;
            _context.RemoveRange(entityToRemove);
        }

        async Task IInternalRepository<T>.RemoveAsync(Guid id)
        {
            var entityToRemove = await DbSet().FindAsync(id);
            if (entityToRemove == null) return;
            _context.RemoveRange(entityToRemove);
        }

        Task IInternalRepository<T>.AddAsync(T entity)
        {
            if (entity == null) throw new Exception("entity cannot be null");
            return DbSet()?.AddAsync(entity).AsTask();
        }

        Task IInternalRepository<T>.AddAsync(List<T> entities)
        {
            if (entities == null) throw new Exception("entities cannot be null");
            return DbSet()?.AddRangeAsync(entities);
        }

        Task IInternalRepository<T>.AddAsync(IEnumerable<T> entities)
        {
            if (entities == null) throw new Exception("entities cannot be null");
            return DbSet()?.AddRangeAsync(entities);
        }

        Task IInternalRepository<T>.AddAsync(IQueryable<T> entities)
        {
            if (entities == null) throw new Exception("entities cannot be null");
            return DbSet()?.AddRangeAsync(entities);
        }

        #endregion


        #region IReadOnlyRepository

        T IReadonlyRepository<T>.Find(Expression<Func<T, bool>> predicate)
        {
            return DbSet()?.FirstOrDefault(predicate);
        }

        T IReadonlyRepository<T>.Find(int id)
        {
            return DbSet()?.Find(id);
        }

        IEnumerable<T> IReadonlyRepository<T>.AsEnumerable()
        {
            return DbSet()?.AsEnumerable();
        }

        IQueryable<T> IReadonlyRepository<T>.AsQueryable()
        {
            return DbSet()?.AsQueryable();
        }

        List<T> IReadonlyRepository<T>.ToList()
        {
            return DbSet()?.ToList();
        }

        int IReadonlyRepository<T>.Count()
        {
            return DbSet()?.Count() ?? 0;
        }

        bool IReadonlyRepository<T>.Exists(Expression<Func<T, bool>> predicate)
        {
            return DbSet()?.Any(predicate) ?? false;
        }

        #endregion

        #region IAsyncReadOnlyRepository

        async Task<IEnumerable<T>> IAsyncReadOnlyRepository<T>.AsEnumerableAsync()
        {
            return await DbSet().ToListAsync();
        }

        Task<List<T>> IAsyncReadOnlyRepository<T>.ToListAsync()
        {
            return DbSet().ToListAsync();
        }

        Task<T> IAsyncReadOnlyRepository<T>.FindAsync(Expression<Func<T, bool>> predicate)
        {
            return DbSet()?.FirstOrDefaultAsync(predicate);
        }

        Task<T> IAsyncReadOnlyRepository<T>.FindAsync(int id)
        {
            return DbSet()?.FindAsync(id).AsTask();
        }

        Task<int> IAsyncReadOnlyRepository<T>.CountAsync()
        {
            return DbSet()?.CountAsync();
        }

        Task<bool> IAsyncReadOnlyRepository<T>.ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return DbSet()?.AnyAsync(predicate);
        }

        T IReadonlyRepository<T>.First()
        {
            return DbSet()?.FirstOrDefault();
        }

        IQueryable<T> IReadonlyRepository<T>.Where(Expression<Func<T, bool>> predicate)
        {
            return DbSet()?.Where(predicate);
        }

        bool IReadonlyRepository<T>.Any(Expression<Func<T, bool>> predicate)
        {
            return DbSet().Any(predicate);
        }

        int IReadonlyRepository<T>.Count(Expression<Func<T, bool>> predicate)
        {
            return DbSet()?.Count(predicate) ?? 0;
        }

        Task<T> IAsyncReadOnlyRepository<T>.FirstAsync()
        {
            return DbSet()?.FirstOrDefaultAsync();
        }

        #endregion

        #region Get
        public IQueryable<T> GetAllIgnoreQueryFilter()
        {
            IQueryable<T> query = DbSet().IgnoreQueryFilters().AsNoTracking();
            return query.AsNoTracking();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<Expression<Func<T, object>>> includes = null, int? page = null,
            int? pageSize = null)
        {
            IQueryable<T> query = DbSet();

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (page != null && pageSize != null && pageSize != 0)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return query.AsNoTracking();
        }

        public IQueryable<T> GetAll(ISpecification<T> spec)
        {
            return ApplySpecification(spec);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<Expression<Func<T, object>>> includes = null, int? page = null,
            int? pageSize = null)
        {
            return await GetAll(filter, orderBy, includes, page, pageSize).ToListAsync();
        }

        public async Task<IReadOnlyCollection<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(DbSet().AsQueryable(), spec);
        }
        #endregion
    }
}
