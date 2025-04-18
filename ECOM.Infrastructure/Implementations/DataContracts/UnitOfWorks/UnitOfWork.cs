using ECOM.Domain.Interfaces.DataContracts;
using ECOM.Infrastructure.Extensions;
using ECOM.Infrastructure.Implementations.DataContracts.Repositories;
using ECOM.Infrastructure.Implementations.DataContracts.Transactions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ECOM.Infrastructure.Implementations.DataContracts.UnitOfWorks
{
	public abstract class UnitOfWork<TContext>(TContext context) : IUnitOfWork where TContext : DbContext
	{
		private bool _disposed = false;
		protected readonly TContext _context = context;
		private readonly Dictionary<Type, object> _repositories = [];

		public IRepository<TEntity> Repository<TEntity>() where TEntity : class
		{
			if (_repositories.TryGetValue(typeof(TEntity), out var repo))
				return (IRepository<TEntity>)repo;

			var repository = new Repository<TEntity>(_context);
			_repositories[typeof(TEntity)] = repository;
			return repository;
		}

		public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		=> await _context.SaveChangesAsync(cancellationToken);

		public async Task<ITransaction> BeginTransactionAsync()
		{
			var transaction = await _context.Database.BeginTransactionAsync();
			return new EfCoreTransaction(transaction);
		}

		#region 🔹 Bulk Execution
		public async Task BulkUpsertAsync<TEntity>(List<TEntity> entities, int batchSize = 5000, int commandTimeoutInMilliseconds = 1000)
		where TEntity : class
		{
			await _context.BulkUpsertAsync(entities, batchSize, commandTimeoutInMilliseconds);
		}

		public async Task BulkDeleteAsync<TEntity>(List<TEntity> entities, int batchSize = 5000, int commandTimeoutInMilliseconds = 1000)
			where TEntity : class
		{
			await _context.BulkDeleteAsync(entities, batchSize, commandTimeoutInMilliseconds);
		}
		#endregion

		#region 🔹 Query Execution

		public async Task<List<TResult>> ExecuteStoredProcedureAsync<TResult>(string storedProcedureName, params object[] parameters)
		{
			using var command = _context.Database.GetDbConnection().CreateCommand();
			command.CommandText = storedProcedureName;
			command.CommandType = System.Data.CommandType.StoredProcedure;
			if (parameters.Length != 0)
			{
				command.Parameters.Clear();
				command.Parameters.AddRange(parameters);
			}
			await _context.Database.OpenConnectionAsync();

			var result = new List<TResult>();
			using var commandResult = await command.ExecuteReaderAsync();

			while (await commandResult.ReadAsync())
			{
				var obj = Activator.CreateInstance<TResult>();
				var propertyInfos = obj?.GetType()?.GetProperties();
				if (propertyInfos != null)
				{
					foreach (PropertyInfo prop in propertyInfos)
					{
						if (!Equals(commandResult[prop.Name], DBNull.Value))
						{
							prop.SetValue(obj, commandResult[prop.Name], null);
						}
					}
					result.Add(obj);
				}
			}
			await _context.Database.CloseConnectionAsync();
			return result;
		}

		public async Task<List<TResult>> ExecuteRawQueryAsync<TResult>(string sqlQuery, params object[] parameters)
		{
			using var command = _context.Database.GetDbConnection().CreateCommand();
			command.CommandText = sqlQuery;
			command.CommandType = System.Data.CommandType.Text;

			if (parameters.Length != 0)
			{
				command.Parameters.Clear();
				command.Parameters.AddRange(parameters);
			}

			await _context.Database.OpenConnectionAsync();

			var result = new List<TResult>();
			using var commandResult = await command.ExecuteReaderAsync();

			while (await commandResult.ReadAsync())
			{
				var obj = Activator.CreateInstance<TResult>();
				var propertyInfos = obj?.GetType()?.GetProperties();
				if (propertyInfos != null)
				{
					foreach (PropertyInfo prop in propertyInfos)
					{
						if (!Equals(commandResult[prop.Name], DBNull.Value))
						{
							prop.SetValue(obj, commandResult[prop.Name], null);
						}
					}
					result.Add(obj);
				}
			}

			await _context.Database.CloseConnectionAsync();
			return result;
		}

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
		~UnitOfWork()
		{
			Dispose(false);
		}

		#endregion
	}
}
