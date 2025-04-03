using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace ECOM.Infrastructure.Persistence.Extensions
{
	public static class DbContextExtensions
	{
		/// <summary>
		/// Detach all entries
		/// </summary>
		/// <param name="dbContext"></param>
		public static void DetachAllEntities(this DbContext dbContext)
		{
			var changedEntriesCopy = dbContext.ChangeTracker.Entries()
				.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
				.ToList();

			foreach (var entry in changedEntriesCopy)
				entry.State = EntityState.Detached;
		}

		/// <summary>
		/// Get Entities from ChangeTracker
		/// </summary>
		/// <param name="dbContext"></param>
		/// <returns></returns>
		public static Dictionary<Type, (List<object> insertedEntities, List<object> updatedEntities, List<object> deletedEntities)> GetEntities(this DbContext dbContext)
		{
			return dbContext.ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted)
										  .GroupBy(x => x.Entity.GetType()).ToDictionary(x => x.Key, x => SplitEntity(x.ToList()));
		}

		/// <summary>
		/// Bulk insert entities
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="dbContext"></param>
		/// <param name="entities"></param>
		/// <param name="connection"></param>
		/// <param name="transaction"></param>
		/// <param name="timeOutInMilieconds"></param>
		/// <param name="batchSize"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static async Task<int> BulkInsertEntitiesAsync<TEntity>(this DbContext dbContext,
			IList<TEntity> entities,
			SqlConnection connection,
			SqlTransaction transaction,
			int timeOutInMilieconds = 100000, int batchSize = 500)
		{
			if (entities == null || entities.Count == 0)
				throw new ArgumentNullException(nameof(entities));

			var tableInfo = dbContext.BuildDataTable(entities.First()).InitDataTable(entities);

			return await BulkCopyData(tableInfo, tableInfo.Table.TableName, connection, transaction, timeOutInMilieconds, batchSize);
		}

		/// <summary>
		/// Bulk update entities
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="dbContext"></param>
		/// <param name="entities"></param>
		/// <param name="connection"></param>
		/// <param name="transaction"></param>
		/// <param name="timeOutInMilieconds"></param>
		/// <param name="batchSize"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static async Task<int> BulkUpdateEntitiesAsync<TEntity>(this DbContext dbContext,
			IList<TEntity> entities,
			SqlConnection connection,
			SqlTransaction transaction,
			int timeOutInMilieconds = 100000, int batchSize = 500)
		{
			var rowEffects = 0;

			if (entities == null || entities.Count == 0)
				throw new ArgumentNullException(nameof(entities));

			var tableInfo = dbContext.BuildDataTable(entities.First()).InitDataTable(entities);

			var tempTableName = $"#tblTemp{Guid.NewGuid()}".Replace("-", string.Empty);

			using (SqlCommand command = new("", connection, transaction))
			{
				command.CommandType = CommandType.Text;
				command.CommandText = tableInfo.BuildTempTableQuery(tempTableName);
				await command.ExecuteNonQueryAsync();

				rowEffects = await BulkCopyData(tableInfo, tempTableName, connection, transaction, batchSize, timeOutInMilieconds);

				command.CommandText = tableInfo.BuildUpdateQuery(tempTableName);
				await command.ExecuteNonQueryAsync();
			}

			return rowEffects;
		}

		public static async Task<int> BulkDeleteEntitiesAsync<TEntity>(this DbContext dbContext,
			IList<TEntity> entities,
			SqlConnection connection,
			SqlTransaction transaction,
			int timeOutInMilieconds = 100000, int batchSize = 500)
		{
			var rowEffects = 0;

			if (entities == null || entities.Count == 0)
				throw new ArgumentNullException(nameof(entities));

			var tableInfo = dbContext.BuildDataTable(entities.First()).InitDataTable(entities);

			var tempTableName = $"#tblTemp{Guid.NewGuid()}".Replace("-", string.Empty);

			using (SqlCommand command = new("", connection, transaction))
			{
				command.CommandType = CommandType.Text;
				command.CommandText = tableInfo.BuildTempTableQuery(tempTableName);
				await command.ExecuteNonQueryAsync();

				rowEffects = await BulkCopyData(tableInfo, tempTableName, connection, transaction, batchSize, timeOutInMilieconds);

				command.CommandText = tableInfo.BuildDeleteQuery(tempTableName);
				await command.ExecuteNonQueryAsync();
			}

			return rowEffects;
		}

		/// <summary>
		/// Get DB value of entity
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="entity"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		private static object GetDbValue<TEntity>(this TEntity entity, string propertyName)
		{
			if (entity == null || string.IsNullOrWhiteSpace(propertyName))
				throw new ArgumentNullException(nameof(propertyName));

			var value = entity.GetType().GetProperty(propertyName)?.GetValue(entity, null);
			return value ?? DBNull.Value;
		}

		/// <summary>
		/// Create temp table for bulk update query
		/// </summary>
		/// <param name="tableInfo"></param>
		/// <param name="tempTableName"></param>
		/// <returns></returns>
		private static string BuildTempTableQuery(this TableInfo tableInfo, string tempTableName)
		{
			var sql = new StringBuilder();

			sql.AppendLine($"IF OBJECT_ID('tempdb..{tempTableName}') IS NOT NULL DROP TABLE {tempTableName}");
			sql.AppendLine($"CREATE TABLE {tempTableName} (");
			for (int i = 0; i < tableInfo.Columns.Count; i++)
			{
				if (i < tableInfo.Columns.Count - 1)
					sql.AppendLine($"   {tableInfo.Columns[i].ColumnName} {tableInfo.Columns[i].ColumnMappingTypeName.ToUpper()},");
				else
					sql.AppendLine($"   {tableInfo.Columns[i].ColumnName} {tableInfo.Columns[i].ColumnMappingTypeName.ToUpper()}");
			}
			sql.AppendLine($")");

			return sql.ToString();
		}

		/// <summary>
		/// Create bulk update query
		/// </summary>
		/// <param name="tableInfo"></param>
		/// <param name="tempTableName"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		private static string BuildUpdateQuery(this TableInfo tableInfo, string tempTableName)
		{
			var primaryKeys = tableInfo.Columns.Where(x => x.IsPrimaryKey).Select(x => x.ColumnName).ToList();
			if (primaryKeys.Count == 0)
				throw new Exception($"Cannot use Bulk action for table {tableInfo.Table.TableName}.");

			var condition = new StringBuilder();
			for (int i = 0; i < primaryKeys.Count; i++)
			{
				if (i < primaryKeys.Count - 1)
					condition.Append($"source.{primaryKeys[i]} = target.{primaryKeys[i]} AND");
				else
					condition.Append($"source.{primaryKeys[i]} = target.{primaryKeys[i]}");
			}

			var updatedFields = tableInfo.Columns.Where(x => x.IsPrimaryKey == false).Select(x => x.ColumnName).ToList();
			if (updatedFields.Count == 0)
				throw new Exception($"Cannot use Bulk action for table {tableInfo.Table.TableName}.");

			var sql = new StringBuilder();
			sql.AppendLine($"UPDATE {tableInfo.Table.TableName} SET");
			sql.AppendLine(string.Join($",{Environment.NewLine}", updatedFields.Select(field => $"{field} = source.{field}").ToArray()));
			sql.AppendLine($"FROM {tableInfo.Table.TableName} as target");
			sql.AppendLine($"INNER JOIN {tempTableName} as source ON {condition.ToString()}");
			sql.AppendLine($"DROP TABLE {tempTableName}");

			return sql.ToString();
		}

		/// <summary>
		/// Create bulk delete query
		/// </summary>
		/// <param name="tableInfo"></param>
		/// <param name="tempTableName"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		private static string BuildDeleteQuery(this TableInfo tableInfo, string tempTableName)
		{
			var primaryKeys = tableInfo.Columns.Where(x => x.IsPrimaryKey).ToList();
			if (primaryKeys.Count == 0)
				throw new Exception($"Cannot use Bulk action for table {tableInfo.Table.TableName}.");

			var condition = new StringBuilder();
			for (int i = 0; i < primaryKeys.Count; i++)
			{
				if (i < primaryKeys.Count - 1)
					condition.Append($"source.{primaryKeys[i].ColumnName} = target.{primaryKeys[i].ColumnName} AND");
				else
					condition.Append($"source.{primaryKeys[i].ColumnName} = target.{primaryKeys[i].ColumnName}");
			}

			var sql = new StringBuilder();
			sql.AppendLine($"DELETE target FROM {tableInfo.Table.TableName} AS target");
			sql.AppendLine($"INNER JOIN {tempTableName} as source ON {condition.ToString()}");
			sql.AppendLine($"DROP TABLE {tempTableName}");
			return sql.ToString();
		}

		/// <summary>
		/// Build DataTable
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="dbContext"></param>
		/// <param name="entity"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		private static TableInfo BuildDataTable<TEntity>(this DbContext dbContext, TEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException(nameof(entity));

			var entityType = dbContext.Model.FindEntityType(entity.GetType());
			if (entityType == null)
				throw new ArgumentNullException(nameof(entityType));

			var schema = entityType.GetSchema();
			var tableName = entityType.GetTableName();

			var tableInfo = new TableInfo
			{
				Table = new DataTable()
				{
					TableName = $"[{schema}].[{tableName}]",
				},
				Columns = []
			};

			var primaryKeys = entityType.FindPrimaryKey()?.Properties?.Select(x => x.Name)?.ToList();

			foreach (var property in entityType.GetProperties())
			{
				var columnName = property.GetColumnName();

				if (string.IsNullOrWhiteSpace(columnName) == false)
				{
					var columnType = entity.GetType()?.GetProperty(columnName)?.PropertyType;
					if (columnType != null)
					{
						var columnMappingTypeName = string.Empty;
						var columnMappingType = property.GetTypeMapping() as object;
						if (columnMappingType != null)
						{
							columnMappingTypeName = columnMappingType.GetType()?.GetProperty("StoreType")?.GetValue(columnMappingType)?.ToString() ?? string.Empty;
							if (string.IsNullOrWhiteSpace(columnMappingTypeName) == false)
							{
								if (columnType.ToString().Contains("System.Nullable"))
									columnMappingTypeName += " null";
								else
									columnMappingTypeName += " not null";
							}
						}

						tableInfo.Columns.Add(new ColumnInfo()
						{
							ColumnType = columnType,
							ColumnName = columnName,
							ColumnMappingTypeName = columnMappingTypeName,
							IsPrimaryKey = primaryKeys?.Contains(columnName) ?? false
						});
						tableInfo.Table.Columns.Add(columnName, Nullable.GetUnderlyingType(columnType) ?? columnType);
					}
				}
			}

			return tableInfo;
		}

		/// <summary>
		/// Init DataTable for entities
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="tableInfo"></param>
		/// <param name="entities"></param>
		/// <returns></returns>
		private static TableInfo InitDataTable<TEntity>(this TableInfo tableInfo, IList<TEntity> entities)
		{
			foreach (var entity in entities)
			{
				var row = tableInfo.Table.NewRow();
				foreach (var column in tableInfo.Columns)
				{
					row[column.ColumnName] = entity.GetDbValue(column.ColumnName);
				}
				tableInfo.Table.Rows.Add(row);
			}
			return tableInfo;
		}

		/// <summary>
		/// Bulk copy data to table
		/// </summary>
		/// <param name="tableInfo"></param>
		/// <param name="destinationTableName"></param>
		/// <param name="connection"></param>
		/// <param name="transaction"></param>
		/// <param name="batchSize"></param>
		/// <param name="timeOutInMilieconds"></param>
		/// <returns></returns>
		private static async Task<int> BulkCopyData(
			TableInfo tableInfo,
			string destinationTableName,
			SqlConnection connection,
			SqlTransaction transaction,
			int batchSize,
			int timeOutInMilieconds)
		{
			using (SqlBulkCopy bulkCopy = new(connection, SqlBulkCopyOptions.Default, transaction))
			{
				bulkCopy.BatchSize = batchSize;
				bulkCopy.BulkCopyTimeout = timeOutInMilieconds / 1000;
				bulkCopy.DestinationTableName = destinationTableName;

				foreach (var column in tableInfo.Columns)
				{
					bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
				}

				await bulkCopy.WriteToServerAsync(tableInfo.Table);
				bulkCopy.Close();
			}
			return tableInfo.Table.Rows.Count;
		}

		/// <summary>
		/// Split Entities
		/// </summary>
		/// <param name="entries"></param>
		/// <returns></returns>
		private static (List<object> insertedEntities, List<object> updatedEntities, List<object> deletedEntities) SplitEntity(IList<EntityEntry> entries)
		{
			var insertedEntities = entries.Where(x => x.State == EntityState.Added).Select(x => x.Entity).ToList();
			var deletedEntities = entries.Where(x => x.State == EntityState.Deleted).Select(x => x.Entity).ToList();
			var updatedEntities = entries.Where(x => x.State == EntityState.Modified).Select(x => x.Entity).ToList();

			return (insertedEntities, updatedEntities, deletedEntities);
		}
	}

	public class ColumnInfo
	{
		public Type ColumnType { get; set; } = typeof(string);
		public string ColumnName { get; set; } = string.Empty;
		public string ColumnMappingTypeName { get; set; } = string.Empty;
		public bool IsPrimaryKey { get; set; }
	}

	public class TableInfo
	{
		public DataTable Table { get; set; } = new DataTable();
		public IList<ColumnInfo> Columns { get; set; } = [];
	}
}
