using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;


namespace ECOM.Infrastructure.Extensions
{
	public static class BulkExtensions
	{
		public static async Task BulkUpsertAsync<TEntity>(this DbContext context, List<TEntity> entities, int batchSize = 5000, int commandTimeoutInMilliseconds = 1000) where TEntity : class
		{
			if (entities == null || entities.Count == 0)
				return;

			var connection = (SqlConnection)context.Database.GetDbConnection();
			if (connection.State != ConnectionState.Open)
				await connection.OpenAsync();

			var createdNewTransaction = false;
			var transaction = context.Database.CurrentTransaction?.GetDbTransaction() as SqlTransaction;
			if (transaction == null)
			{
				transaction = await connection.BeginTransactionAsync() as SqlTransaction ?? throw new Exception($"Can't create new transaction!");
				createdNewTransaction = true;
			}

			try
			{
				var entityType = context.Model.FindEntityType(typeof(TEntity))
						?? throw new InvalidOperationException($"Entity {typeof(TEntity).Name} not found in DbContext metadata.");

				var dbColumns = entityType.GetProperties()
								  .Where(p => !p.IsShadowProperty())
								  .Select(p => p.GetColumnName())
								  .ToList() ?? [];

				var tableName = typeof(TEntity).Name;
				var tempTableName = $"#{tableName}_{Guid.NewGuid().ToString().Replace("-", string.Empty)}_Temp";

				// Detect primary keys
				var primaryKeys = GetPrimaryKeyColumns<TEntity>(context);
				if (primaryKeys.Count == 0)
					throw new InvalidOperationException($"Cannot detect primary key(s) for entity {typeof(TEntity).Name}");

				// 1. Create temp table
				await CreateTempTableAsync(connection, transaction, tableName, tempTableName);

				// 2. Build Data Table
				var dataTable = ToDataTable(entities, dbColumns, context);

				// 3. Bulk copy into temp table
				using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
				{
					bulkCopy.DestinationTableName = tempTableName;
					bulkCopy.BatchSize = batchSize;
					bulkCopy.BulkCopyTimeout = commandTimeoutInMilliseconds;

					foreach (var column in dbColumns)
					{
						bulkCopy.ColumnMappings.Add(column, column);
					}

					await bulkCopy.WriteToServerAsync(dataTable);
				}

				// 3. Merge temp table into main table
				var mergeSql = GenerateMergeSql<TEntity>(tableName, tempTableName, primaryKeys, dbColumns);

				using (var command = new SqlCommand(mergeSql, connection, transaction))
				{
					await command.ExecuteNonQueryAsync();
				}

				if (createdNewTransaction)
					await transaction.CommitAsync();
			}
			catch
			{
				if (createdNewTransaction)
					await transaction.RollbackAsync();
				throw;
			}
		}

		public static async Task BulkDeleteAsync<TEntity>(this DbContext context, List<TEntity> entities, int batchSize = 5000, int commandTimeoutInMilliseconds = 1000) where TEntity : class
		{
			if (entities == null || entities.Count == 0)
				return;

			var connection = (SqlConnection)context.Database.GetDbConnection();
			if (connection.State != ConnectionState.Open)
				await connection.OpenAsync();

			var createdNewTransaction = false;
			var transaction = context.Database.CurrentTransaction?.GetDbTransaction() as SqlTransaction;
			if (transaction == null)
			{
				transaction = await connection.BeginTransactionAsync() as SqlTransaction ?? throw new Exception($"Can't create new transaction!");
				createdNewTransaction = true;
			}

			try
			{
				var tableName = typeof(TEntity).Name;
				var tempTableName = $"#{tableName}_{Guid.NewGuid().ToString().Replace("-", string.Empty)}_Temp";

				var primaryKeys = GetPrimaryKeyColumns<TEntity>(context);
				if (primaryKeys.Count == 0)
					throw new InvalidOperationException($"Cannot detect primary key(s) for entity {typeof(TEntity).Name}");

				await CreateTempTableAsync(connection, transaction, tableName, tempTableName);

				using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
				{
					bulkCopy.DestinationTableName = tempTableName;
					bulkCopy.BatchSize = batchSize;
					bulkCopy.BulkCopyTimeout = commandTimeoutInMilliseconds;

					using var dataTable = ToDataTablePrimaryKeys(entities, primaryKeys);
					await bulkCopy.WriteToServerAsync(dataTable);
				}

				var deleteSql = GenerateDeleteSql(tableName, tempTableName, primaryKeys);

				using (var command = new SqlCommand(deleteSql, connection, transaction))
				{
					await command.ExecuteNonQueryAsync();
				}

				if (createdNewTransaction)
					await transaction.CommitAsync();
			}
			catch
			{
				if (createdNewTransaction)
					await transaction.RollbackAsync();
				throw;
			}
		}

		public static void DetachAllEntities(this DbContext dbContext)
		{
			var changedEntriesCopy = dbContext.ChangeTracker.Entries()
				.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
				.ToList();

			foreach (var entry in changedEntriesCopy)
				entry.State = EntityState.Detached;
		}

		private static async Task CreateTempTableAsync(SqlConnection connection, SqlTransaction transaction, string sourceTable, string tempTable)
		{
			var sql = $"SELECT TOP 0 * INTO {tempTable} FROM {sourceTable};";
			using var command = new SqlCommand(sql, connection, transaction);
			await command.ExecuteNonQueryAsync();
		}

		private static List<string> GetPrimaryKeyColumns<TEntity>(DbContext context) where TEntity : class
		{
			var entityType = context.Model.FindEntityType(typeof(TEntity))
				?? throw new InvalidOperationException($"Entity type not found in {context.GetType().Name} Model.");

			var primaryKey = entityType.FindPrimaryKey()
				?? throw new InvalidOperationException($"Primary key not found.");

			return primaryKey.Properties.Select(p => p.Name).ToList();
		}

		private static string GenerateMergeSql<TEntity>(string mainTable, string tempTable, List<string> primaryKeys, List<string> dbColumns) where TEntity : class
		{
			var onCondition = string.Join(" AND ", primaryKeys.Select(pk => $"Target.{pk} = Source.{pk}"));
			var updateSet = dbColumns
				.Where(p => !primaryKeys.Contains(p))
				.Select(p => $"Target.{p} = Source.{p}")
				.ToList();

			var insertColumns = string.Join(", ", dbColumns);
			var insertValues = string.Join(", ", dbColumns.Select(p => $"Source.{p}"));

			return $@"
					MERGE INTO {mainTable} AS Target
					USING {tempTable} AS Source
					ON {onCondition}
					WHEN MATCHED THEN 
						UPDATE SET {string.Join(", ", updateSet)}
					WHEN NOT MATCHED BY TARGET THEN
						INSERT ({insertColumns})
						VALUES ({insertValues});
					";
		}

		private static string GenerateDeleteSql(string mainTable, string tempTable, List<string> primaryKeys)
		{
			var joinCondition = string.Join(" AND ", primaryKeys.Select(pk => $"Target.{pk} = Source.{pk}"));

			return $@"
					DELETE Target
					FROM {mainTable} AS Target
					INNER JOIN {tempTable} AS Source
					ON {joinCondition};
					";
		}

		private static DataTable ToDataTable<TEntity>(List<TEntity> entities, List<string> dbColumns, DbContext context) where TEntity : class
		{
			var table = new DataTable();
			var entityType = context.Model.FindEntityType(typeof(TEntity));
			var props = typeof(TEntity).GetProperties()
				.Where(p => dbColumns.Contains(p.Name))
				.ToList();

			foreach (var prop in props)
			{
				var propertyType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
				var column = table.Columns.Add(prop.Name, propertyType);

				if (propertyType == typeof(string))
				{
					var dbProperty = entityType?.FindProperty(prop.Name);
					if (dbProperty != null)
					{
						var maxLength = dbProperty.GetMaxLength();
						if (maxLength.HasValue)
							column.MaxLength = maxLength.Value;
					}
				}
			}

			foreach (var entity in entities)
			{
				var values = props.Select(p => p.GetValue(entity) ?? DBNull.Value).ToArray();
				table.Rows.Add(values);
			}

			return table;
		}

		private static DataTable ToDataTablePrimaryKeys<TEntity>(List<TEntity> entities, List<string> primaryKeys) where TEntity : class
		{
			var table = new DataTable();
			var properties = typeof(TEntity).GetProperties()
				.Where(p => p.CanRead && p.CanWrite && primaryKeys.Contains(p.Name))
				.ToList();

			foreach (var prop in properties)
			{
				table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
			}

			foreach (var entity in entities)
			{
				var values = properties.Select(p => p.GetValue(entity) ?? DBNull.Value).ToArray();
				table.Rows.Add(values);
			}

			return table;
		}
	}
}
