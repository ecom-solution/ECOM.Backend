using ECOM.Domain.Interfaces.DataContracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECOM.Infrastructure.Implementations.DataContracts.Repositories
{
	public class EfCoreRepository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		private bool _disposed = false;
		protected readonly DbContext _context;
		protected readonly DbSet<TEntity> _dbSet;

		public EfCoreRepository(DbContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_dbSet = _context.Set<TEntity>();
		}

        #region 🔹 Creation

        /// <inheritdoc />
        public void Insert(TEntity entity) 
            => _dbSet.Add(entity);

        /// <inheritdoc />
        public void InsertRange(IEnumerable<TEntity> entities) 
            => _dbSet.AddRange(entities);

        /// <inheritdoc />
        public async Task InsertAsync(TEntity entity) 
            => await _dbSet.AddAsync(entity);

        /// <inheritdoc />
        public async Task InsertRangeAsync(IEnumerable<TEntity> entities) 
            => await _dbSet.AddRangeAsync(entities);

        #endregion

        #region 🔹 Retrieval (Synchronous)

        /// <inheritdoc />
        public IQueryable<TEntity> Query(bool isNoTracking = false) 
            => isNoTracking ? _dbSet.AsNoTracking() : _dbSet;

        /// <inheritdoc />
        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate, bool isNoTracking = false) 
            => isNoTracking ? _dbSet.AsNoTracking().Where(predicate) : _dbSet.Where(predicate);

        #endregion

        #region 🔹 Retrieval (Asynchronous - Single Result)

        /// <inheritdoc />
        public async Task<TEntity?> GetByIdAsync(Guid id) 
            => await _dbSet.FindAsync(id);

        /// <inheritdoc />
        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate) 
            => await _dbSet.FirstOrDefaultAsync(predicate);

        /// <inheritdoc />
        public async Task<TEntity?> FirstOrDefaultAsync(IQueryable<TEntity> query) 
            => await query.FirstOrDefaultAsync();

        #endregion

        #region 🔹 Retrieval (Asynchronous - Collection)

        /// <inheritdoc />
        public async Task<IEnumerable<TEntity>> GetAllAsync() 
            => await _dbSet.ToListAsync();

        /// <inheritdoc />
        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate) 
            => await _dbSet.Where(predicate).ToListAsync();

        /// <inheritdoc />
        public async Task<List<TEntity>> ToListAsync(IQueryable<TEntity> query) 
            => await query.ToListAsync();

        /// <inheritdoc />
        public async Task<List<TOut>> ToListAsync<TOut>(IQueryable<TEntity> query, Expression<Func<TEntity, TOut>> selector) 
            => await query.Select(selector).ToListAsync();

        #endregion

        #region 🔹 Update

        /// <inheritdoc />
        public void Update(TEntity entity) 
            => _context.Entry(entity).State = EntityState.Modified;

        /// <inheritdoc />
        public void UpdateRange(IEnumerable<TEntity> entities) 
            => _dbSet.UpdateRange(entities);

        #endregion

        #region 🔹 Deletion

        /// <inheritdoc />
        public void Delete(TEntity entity) 
            => _dbSet.Remove(entity);

        /// <inheritdoc />
        public void DeleteRange(IEnumerable<TEntity> entities) 
            => _dbSet.RemoveRange(entities);

        /// <inheritdoc />
        public void Delete(Expression<Func<TEntity, bool>> predicate) 
            => _dbSet.Where(predicate).ToList().ForEach(entity => _dbSet.Remove(entity));

        /// <inheritdoc />
        public async Task DeleteByIdAsync(Guid id)
        {
            var entityToDelete = await _dbSet.FindAsync(id);
            if (entityToDelete != null)
            {
                _dbSet.Remove(entityToDelete);
            }
        }

        #endregion

        #region 🔹 Utility (Asynchronous)

        /// <inheritdoc />
        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate) 
            => await _dbSet.AnyAsync(predicate);

        /// <inheritdoc />
        public async Task<bool> AnyAsync(IQueryable<TEntity> query) 
            => await query.AnyAsync();

        /// <inheritdoc />
        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate) 
            => await _dbSet.CountAsync(predicate);

        /// <inheritdoc />
        public async Task<int> CountAsync<TOut>(IQueryable<TEntity> query, Expression<Func<TEntity, TOut>> selector)
            => await query.Select(selector).CountAsync();

        #endregion

        #region 🔹 Eager Loading & Ordering (Query Manipulation)

        /// <inheritdoc />
        public IQueryable<TEntity> Include<TProperty>(Expression<Func<TEntity, TProperty>> navigationPropertyPath) 
            => _dbSet.Include(navigationPropertyPath);

        /// <inheritdoc />
        public IOrderedQueryable<TEntity> OrderBy<TKey>(IQueryable<TEntity> query, Expression<Func<TEntity, TKey>> keySelector) 
            => query.OrderBy(keySelector);

        /// <inheritdoc />
        public IOrderedQueryable<TEntity> OrderByDescending<TKey>(IQueryable<TEntity> query, Expression<Func<TEntity, TKey>> keySelector) 
            => query.OrderByDescending(keySelector);

        #endregion

        #region 🔹 Dispose Pattern

        /// <summary>
        /// Disposes the context and releases resources.
        /// </summary>
        public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Asynchronously disposes the context and releases resources.
		/// </summary>
		public async ValueTask DisposeAsync()
		{
			await DisposeAsyncCore();
			Dispose(false);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Protected method to implement dispose logic.
		/// </summary>
		/// <param name="disposing">Indicates whether it's disposing managed resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					// Dispose managed resources
					_context.Dispose();
				}
				_disposed = true;
			}
		}

		/// <summary>
		/// Protected method to implement async dispose logic.
		/// </summary>
		/// <returns>A task representing the asynchronous operation.</returns>
		protected virtual async ValueTask DisposeAsyncCore()
		{
			if (!_disposed)
			{
				if (_context is IAsyncDisposable asyncDisposable)
				{
					await asyncDisposable.DisposeAsync();
				}
				else
				{
					_context.Dispose();
				}
				_disposed = true;
			}
		}

		/// <summary>
		/// Destructor to ensure proper cleanup of unmanaged resources.
		/// </summary>
		~EfCoreRepository()
		{
			Dispose(false);
		}

		#endregion
	}
}
