using ECOM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECOM.Infrastructure.Persistence.Implementations.Repositories
{
	
	public class Repository<TContext, TEntity> : IRepository<TContext, TEntity>
	where TContext : DbContext
	where TEntity : class
	{
		private bool _disposed = false;
		protected readonly TContext _context;
		protected readonly DbSet<TEntity> _dbSet;

		public Repository(TContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_dbSet = _context.Set<TEntity>();
		}

		#region Create (Insert)
		public void Insert(TEntity entity) => _dbSet.Add(entity);
		public void InsertRange(IEnumerable<TEntity> entities) => _dbSet.AddRange(entities);
		public async Task InsertAsync(TEntity entity) => await _dbSet.AddAsync(entity);
		public async Task InsertRangeAsync(IEnumerable<TEntity> entities) => await _dbSet.AddRangeAsync(entities);
		#endregion

		#region Read (Retrieve)
		public IQueryable<TEntity> Query(bool isNoTracking = false)
			=> isNoTracking ? _dbSet.AsNoTracking() : _dbSet;

		public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate, bool isNoTracking = false)
			=> isNoTracking ? _dbSet.Where(predicate).AsNoTracking() : _dbSet.Where(predicate);

		public async Task<TEntity?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

		public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbSet.ToListAsync();

		public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
			=> await _dbSet.Where(predicate).ToListAsync();

		public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
			=> await _dbSet.FirstOrDefaultAsync(predicate);
		#endregion

		#region Update
		public void Update(TEntity entity) => _dbSet.Update(entity);
		public void UpdateRange(IEnumerable<TEntity> entities) => _dbSet.UpdateRange(entities);
		#endregion

		#region Delete
		public void Delete(TEntity entity) => _dbSet.Remove(entity);
		public void DeleteRange(IEnumerable<TEntity> entities) => _dbSet.RemoveRange(entities);

		public void Delete(Expression<Func<TEntity, bool>> predicate)
		{
			var entities = _dbSet.Where(predicate);
			_dbSet.RemoveRange(entities);
		}

		public async Task DeleteByIdAsync(Guid id)
		{
			var entity = await GetByIdAsync(id);
			if (entity != null) _dbSet.Remove(entity);
		}
		#endregion

		#region Utility Methods
		public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
			=> await _dbSet.AnyAsync(predicate);

		public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
			=> await _dbSet.CountAsync(predicate);
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
		~Repository()
		{
			Dispose(false);
		}

		#endregion
	}
}
