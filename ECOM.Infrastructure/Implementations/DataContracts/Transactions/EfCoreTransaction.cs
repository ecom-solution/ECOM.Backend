using ECOM.Domain.Interfaces.DataContracts;
using Microsoft.EntityFrameworkCore.Storage;

namespace ECOM.Infrastructure.Implementations.DataContracts.Transactions
{
    public class EfCoreTransaction(IDbContextTransaction transaction) : ITransaction
    {
        private bool _disposed = false;
        private readonly IDbContextTransaction _transaction = transaction;

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _transaction.CommitAsync(cancellationToken);
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            await _transaction.RollbackAsync(cancellationToken);
        }

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
                    _transaction.Dispose();
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
                if (_transaction is IAsyncDisposable asyncDisposable)
                {
                    await asyncDisposable.DisposeAsync();
                }
                else
                {
                    _transaction.Dispose();
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// Destructor to ensure proper cleanup of unmanaged resources.
        /// </summary>
        ~EfCoreTransaction()
        {
            Dispose(false);
        }

        #endregion
    }
}
