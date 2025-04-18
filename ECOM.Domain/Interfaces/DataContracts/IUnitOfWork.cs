namespace ECOM.Domain.Interfaces.DataContracts
{
	public interface IUnitOfWork : IDisposable, IAsyncDisposable
	{
		#region 🔹 Save Changes

		/// <summary>
		/// Saves all changes made in the current unit of work asynchronously.
		/// </summary>
		/// <param name="cancellationToken">A token to cancel the operation.</param>
		/// <returns>The number of state entries written to the database.</returns>
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

		#endregion
		
		#region 🔹 Transaction

		/// <summary>
		/// Begins a database transaction.
		/// </summary>
		/// <returns>Returns an <see cref="ITransaction"/> that must be disposed or committed.</returns>
		Task<ITransaction> BeginTransactionAsync();
		#endregion

		#region 🔹 Repository Access

		/// <summary>
		/// Provides access to a generic repository for a specific entity type.
		/// </summary>
		/// <typeparam name="TEntity">The entity type to retrieve the repository for.</typeparam>
		/// <returns>An instance of <see cref="IRepository{TContext, TEntity}"/> for managing entity operations.</returns>
		IRepository<TEntity> Repository<TEntity>() where TEntity : class;

		#endregion

		#region 🔹 Bulk Execution

		/// <summary>
		/// Performs a bulk insert or update (upsert) operation for a list of entities.
		/// Automatically detects primary keys and merges records based on those keys.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity to be upserted.</typeparam>
		/// <param name="entities">The list of entities to be upserted into the database.</param>
		/// <param name="batchSize">The number of records to process per batch. Default is 5000.</param>
		/// <param name="commandTimeoutInMilliseconds">The SQL command timeout in milliseconds. Default is 1000 ms.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		Task BulkUpsertAsync<TEntity>(
			List<TEntity> entities,
			int batchSize = 5000,
			int commandTimeoutInMilliseconds = 1000
		) where TEntity : class;

		/// <summary>
		/// Performs a bulk delete operation for a list of entities based on their primary keys.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity to be deleted.</typeparam>
		/// <param name="entities">The list of entities to be deleted from the database.</param>
		/// <param name="batchSize">The number of records to process per batch. Default is 5000.</param>
		/// <param name="commandTimeoutInMilliseconds">The SQL command timeout in milliseconds. Default is 1000 ms.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		Task BulkDeleteAsync<TEntity>(
			List<TEntity> entities,
			int batchSize = 5000,
			int commandTimeoutInMilliseconds = 1000
		) where TEntity : class;

		#endregion

		#region 🔹 Query Execution

		/// <summary>
		/// Executes a stored procedure asynchronously and retrieves the results.
		/// </summary>
		/// <typeparam name="TResult">The type of the result expected from the stored procedure.</typeparam>
		/// <param name="storedProcedureName">The name of the stored procedure to execute.</param>
		/// <param name="parameters">The parameters required by the stored procedure.</param>
		/// <returns>A list of results of type <typeparamref name="TResult"/>.</returns>
		Task<List<TResult>> ExecuteStoredProcedureAsync<TResult>(string storedProcedureName, params object[] parameters);

		/// <summary>
		/// Executes a raw SQL query asynchronously and retrieves the results.
		/// </summary>
		/// <typeparam name="TResult">The type of the result expected from the query.</typeparam>
		/// <param name="sqlQuery">The SQL query to execute.</param>
		/// <param name="parameters">The parameters required by the query.</param>
		/// <returns>A list of results of type <typeparamref name="TResult"/>.</returns>
		Task<List<TResult>> ExecuteRawQueryAsync<TResult>(string sqlQuery, params object[] parameters);
		#endregion
	}
}
