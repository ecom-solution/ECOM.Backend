using ECOM.Domain.Interfaces.Repositories;
using ECOM.Infrastructure.Persistence.Extensions;
using ECOM.Shared.Utilities.Helpers;
using ECOM.Shared.Utilities.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace ECOM.Infrastructure.Persistence.Implementations.Repositories
{
	public class UnitOfWork<TContext>(TContext context, IOptions<AppSettings> appSettings) : IUnitOfWork<TContext> where TContext : DbContext
	{
		private bool _disposed = false;
		private readonly TContext _dbContext = context;
		private readonly AppSettings _appSettings = appSettings.Value;
		private readonly Dictionary<Type, object> _repositories = [];

		#region 🔹 Save Changes

		public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default, bool isPartOfTransaction = false)
		{
			return await RetryHelper.RetryAsync(async () =>
			{
				var isSaveChange = _dbContext.ChangeTracker.Entries().Any(x => x.State == EntityState.Added
																			|| x.State == EntityState.Deleted
																			|| x.State == EntityState.Modified);
				if (isSaveChange)
				{
					if (isPartOfTransaction)
					{
						return await _dbContext.SaveChangesAsync(cancellationToken);
					}
					using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
					try
					{
						var result = await _dbContext.SaveChangesAsync(cancellationToken);
						await transaction.CommitAsync(cancellationToken);

						_dbContext.DetachAllEntities();

						return result;
					}
					catch (Exception)
					{
						await transaction.RollbackAsync(cancellationToken);
						throw;
					}
				}
				return 0;
			}, TimeSpan.FromSeconds(_appSettings.DbContext.Retry.IntervalInSeconds), _appSettings.DbContext.Retry.MaxAttemptCount);
		}

		public async Task<int> BulkSaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return await RetryHelper.RetryAsync(async () =>
			{
				var rowEffects = 0;
				var isSaveChange = _dbContext.ChangeTracker.Entries().Any(x => x.State == EntityState.Added
																			|| x.State == EntityState.Deleted
																			|| x.State == EntityState.Modified);
				if (isSaveChange)
				{
					if (_dbContext.Database.GetDbConnection() is SqlConnection connection)
					{
						if (connection.State != System.Data.ConnectionState.Open)
							await connection.OpenAsync();

						using var transaction = await connection.BeginTransactionAsync(cancellationToken) as SqlTransaction;
						if (transaction != null)
						{
							try
							{
								var entities = _dbContext.GetEntities();
								if (entities.Any())
								{
									foreach (var item in entities)
									{
										var insertedEntities = item.Value.insertedEntities;
										if (insertedEntities != null && insertedEntities.Count != 0)
										{
											rowEffects += await _dbContext.BulkInsertEntitiesAsync(insertedEntities, connection, transaction, _appSettings.DbContext.Bulk.CmdTimeOutInMiliseconds, _appSettings.DbContext.Bulk.BatchSize);
										}

										var updatedEntities = item.Value.updatedEntities;
										if (updatedEntities != null && updatedEntities.Count != 0)
										{
											rowEffects += await _dbContext.BulkUpdateEntitiesAsync(updatedEntities, connection, transaction, _appSettings.DbContext.Bulk.CmdTimeOutInMiliseconds, _appSettings.DbContext.Bulk.BatchSize);
										}

										var deletedEntities = item.Value.deletedEntities;
										if (deletedEntities != null && deletedEntities.Count != 0)
										{
											rowEffects += await _dbContext.BulkDeleteEntitiesAsync(deletedEntities, connection, transaction, _appSettings.DbContext.Bulk.CmdTimeOutInMiliseconds, _appSettings.DbContext.Bulk.BatchSize);
										}
									}
								}

								await transaction.CommitAsync(cancellationToken);

								_dbContext.DetachAllEntities();
							}
							catch (Exception)
							{
								await transaction.RollbackAsync(cancellationToken);
								throw;
							}

						}
					}
				}
				return rowEffects;
			}, TimeSpan.FromSeconds(_appSettings.DbContext.Retry.IntervalInSeconds), _appSettings.DbContext.Retry.MaxAttemptCount);
		}

		#endregion

		#region 🔹 Repository Access

		public IRepository<TContext, TEntity> Repository<TEntity>() where TEntity : class
		{
			var entityType = typeof(TEntity);
			if (!_repositories.ContainsKey(typeof(TEntity)))
			{
				_repositories.Add(entityType, new Repository<TContext, TEntity>(_dbContext));
			}

			return (IRepository<TContext, TEntity>)_repositories[typeof(TEntity)];
		}

		#endregion

		#region 🔹 Context Access

		public TContext GetContext()
		{
			return _dbContext;
		}

		#endregion

		#region 🔹 Query Execution

		public async Task<List<TResult>> ExecuteStoredProcedureAsync<TResult>(string storedProcedureName, params object[] parameters)
		{
			using var command = _dbContext.Database.GetDbConnection().CreateCommand();
			command.CommandText = storedProcedureName;
			command.CommandType = System.Data.CommandType.StoredProcedure;
			if (parameters.Length != 0)
			{
				command.Parameters.Clear();
				command.Parameters.AddRange(parameters);
			}
			await _dbContext.Database.OpenConnectionAsync();

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
			await _dbContext.Database.CloseConnectionAsync();
			return result;
		}

		public async Task<List<TResult>> ExecuteRawQueryAsync<TResult>(string sqlQuery, params object[] parameters)
		{
			using var command = _dbContext.Database.GetDbConnection().CreateCommand();
			command.CommandText = sqlQuery;
			command.CommandType = System.Data.CommandType.Text;

			if (parameters.Length != 0)
			{
				command.Parameters.Clear();
				command.Parameters.AddRange(parameters);
			}

			await _dbContext.Database.OpenConnectionAsync();

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

			await _dbContext.Database.CloseConnectionAsync();
			return result;
		}

		#endregion

		#region 🔹 Change Tracker

		public void ClearChangeTracker()
		{
			var entries = _dbContext.ChangeTracker.Entries().ToList();
			foreach (var entry in entries)
			{
				entry.State = EntityState.Detached;
			}
		}

		public void DetachEntity(object entity)
		{
			var entry = _dbContext.Entry(entity);
			entry.State = EntityState.Detached;
		}

		#endregion

		#region 🔹 Execution Control

		public void SetTimeout(int seconds)
		{
			_dbContext.Database.SetCommandTimeout(seconds);
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
					_dbContext.Dispose();
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
				if (_dbContext is IAsyncDisposable asyncDisposable)
				{
					await asyncDisposable.DisposeAsync();
				}
				else
				{
					_dbContext.Dispose();
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
