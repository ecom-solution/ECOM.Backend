namespace ECOM.Domain.Interfaces.DataContracts
{
	public interface ITransaction : IDisposable, IAsyncDisposable
	{
		/// <summary>
		/// Asynchronously commits all operations within the current transaction.
		/// This method should be called after all transactional operations have
		/// completed successfully to persist changes to the database.
		/// </summary>
		/// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
		/// <returns>A task representing the asynchronous commit operation.</returns>
		Task CommitAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Asynchronously rolls back all operations within the current transaction.
		/// This method should be invoked if any operation within the transaction
		/// fails, ensuring that no partial changes are persisted to the database.
		/// </summary>
		/// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
		/// <returns>A task representing the asynchronous rollback operation.</returns>
		Task RollbackAsync(CancellationToken cancellationToken = default);
	}
}
