using ECOM.App.Interfaces.Loggings;
using ECOM.Domain.Interfaces.DataContracts;
using ECOM.Domain.Interfaces.Seeders;
using ECOM.Shared.Library.Consts;
using ECOM.Shared.Library.Models.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ECOM.Infrastructure.Implementations.Seeders
{
	public abstract class DbSeeder(
		ILog logger,
		IOptions<AppSettings> appSettings,
		[FromKeyedServices(DatabaseConstants.Main)] IUnitOfWork mainUnitOfWork) : IDbSeeder
	{
		protected readonly ILog _logger = logger;
		protected readonly AppSettings _appSettings = appSettings.Value;
		protected readonly IUnitOfWork _mainUnitOfWork = mainUnitOfWork;

		public abstract int Priority { get; }

		public abstract Task SeedAsync();

		public virtual async Task<List<TEntity>> ReadAsync<TEntity>(string relativePath)
		{
			var baseDirectory = AppContext.BaseDirectory;
			var fullPath = Path.Combine(baseDirectory, relativePath);

			if (!File.Exists(fullPath))
				throw new FileNotFoundException($"Seeder file not found at path: {fullPath}");

			await using var stream = File.OpenRead(fullPath);

			var data = await JsonSerializer.DeserializeAsync<List<TEntity>>(stream, GetJsonSerializerOptions()) ?? [];

			return data;
		}
		public virtual JsonSerializerOptions GetJsonSerializerOptions()
		{
			return new()
			{
				PropertyNameCaseInsensitive = true,
				WriteIndented = false
			};
		}
	}
}
