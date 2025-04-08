namespace ECOM.Domain.Interfaces.Repositories
{
	public interface IUnitOfWork<TContext> : IDisposable, IAsyncDisposable
	{
		#region 🔹 Save Changes

		/// <summary>
		/// Saves all changes made in the current unit of work asynchronously.
		/// If the operation is inside an existing transaction, it will not commit the transaction.
		/// </summary>
		/// <param name="cancellationToken">A token to cancel the operation.</param>
		/// <param name="isPartOfTransaction">
		/// Indicates whether this save operation is part of an already opened transaction.
		/// If <c>true</c>, changes will be saved but not committed.
		/// </param>
		/// <returns>The number of state entries written to the database.</returns>
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default, bool isPartOfTransaction = false);
		#endregion

		#region 🔹 Repository Access

		/// <summary>
		/// Provides access to a generic repository for a specific entity type.
		/// </summary>
		/// <typeparam name="TEntity">The entity type to retrieve the repository for.</typeparam>
		/// <returns>An instance of <see cref="IRepository{TContext, TEntity}"/> for managing entity operations.</returns>
		IRepository<TContext, TEntity> Repository<TEntity>() where TEntity : class;

		#endregion

		#region 🔹 Context Access

		/// <summary>
		/// Retrieves the current database context instance.
		/// This allows direct access to the DbContext for advanced operations.
		/// </summary>
		/// <returns>The instance of <typeparamref name="TContext"/> representing the database context.</returns>
		TContext GetContext();

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

		#region 🔹 Change Tracker

		/// <summary>
		/// Clears all tracked entities from the DbContext, ensuring a fresh state.
		/// Useful when working with detached entities or after bulk operations.
		/// </summary>
		void ClearChangeTracker();

		/// <summary>
		/// Detaches a specific entity from the DbContext, preventing further tracking.
		/// This is helpful when working with memory-sensitive operations or when reloading data.
		/// </summary>
		/// <param name="entity">The entity to detach from tracking.</param>
		void DetachEntity(object entity);

		#endregion

		#region 🔹 Execution Control

		/// <summary>
		/// Sets the execution timeout for database operations.
		/// Useful for long-running queries that require extended execution time.
		/// </summary>
		/// <param name="seconds">The timeout duration in seconds.</param>
		void SetTimeout(int seconds);

		#endregion
	}

}
