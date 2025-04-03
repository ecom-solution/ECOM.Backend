using System.Linq.Expressions;

namespace ECOM.Domain.Interfaces.Repositories
{
	public interface IRepository<TContext, TEntity> : IDisposable, IAsyncDisposable
	{
		#region 🔹 Create (Insert)

		/// <summary>
		/// Insert a new entity.
		/// </summary>
		/// <param name="entity">The entity to insert.</param>
		void Insert(TEntity entity);

		/// <summary>
		/// Insert multiple entities.
		/// </summary>
		/// <param name="entities">The collection of entities to insert.</param>
		void InsertRange(IEnumerable<TEntity> entities);

		/// <summary>
		/// Insert a new entity asynchronously.
		/// </summary>
		/// <param name="entity">The entity to insert.</param>
		Task InsertAsync(TEntity entity);

		/// <summary>
		/// Insert multiple entities asynchronously.
		/// </summary>
		/// <param name="entities">The collection of entities to insert.</param>
		Task InsertRangeAsync(IEnumerable<TEntity> entities);

		#endregion

		#region 🔹 Read (Retrieve)

		/// <summary>
		/// Get all entities.
		/// </summary>
		/// <returns>A collection of all entities.</returns>
		IQueryable<TEntity> Query(bool isNoTracking = false);

		/// <summary>
		/// Get entities that match a given condition.
		/// </summary>
		/// <param name="predicate">The condition to filter entities.</param>
		IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate, bool isNoTracking = false);

		/// <summary>
		/// Get an entity by its ID asynchronously.
		/// </summary>
		/// <param name="id">The ID of the entity.</param>
		/// <returns>The entity if found, otherwise null.</returns>
		Task<TEntity?> GetByIdAsync(Guid id);

		/// <summary>
		/// Get all entities asynchronously.
		/// </summary>
		/// <returns>A collection of all entities.</returns>
		Task<IEnumerable<TEntity>> GetAllAsync();

		/// <summary>
		/// Find entities based on a predicate asynchronously.
		/// </summary>
		/// <param name="predicate">The condition to filter entities.</param>
		Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Get the first entity matching the condition asynchronously.
		/// </summary>
		/// <param name="predicate">The condition to filter entities.</param>
		Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

		#endregion

		#region 🔹 Update

		/// <summary>
		/// Update an existing entity.
		/// </summary>
		/// <param name="entity">The entity to update.</param>
		void Update(TEntity entity);

		/// <summary>
		/// Update multiple entities.
		/// </summary>
		/// <param name="entities">The collection of entities to update.</param>
		void UpdateRange(IEnumerable<TEntity> entities);

		#endregion

		#region 🔹 Delete

		/// <summary>
		/// Delete an entity.
		/// </summary>
		/// <param name="entity">The entity to delete.</param>
		void Delete(TEntity entity);

		/// <summary>
		/// Delete multiple entities.
		/// </summary>
		/// <param name="entities">The collection of entities to delete.</param>
		void DeleteRange(IEnumerable<TEntity> entities);

		/// <summary>
		/// Delete entities based on a condition.
		/// </summary>
		/// <param name="predicate">The condition to filter entities to delete.</param>
		void Delete(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Delete an entity by its ID asynchronously.
		/// </summary>
		/// <param name="id">The ID of the entity to delete.</param>
		Task DeleteByIdAsync(Guid id);

		#endregion

		#region 🔹 Utility Methods

		/// <summary>
		/// Check if any entity matches the given condition.
		/// </summary>
		/// <param name="predicate">The condition to check.</param>
		Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Count the number of entities that match the given condition asynchronously.
		/// </summary>
		/// <param name="predicate">The condition to count entities.</param>
		Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

		#endregion
	}

}
