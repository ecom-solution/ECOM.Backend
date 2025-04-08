using ECOM.Domain.Interfaces.Repositories;
using ECOM.Domain.Interfaces.Seeders;
using ECOM.Infrastructure.Logging.Interfaces;
using ECOM.Infrastructure.Persistence.Main;
using ECOM.Shared.Utilities.Settings;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ECOM.Infrastructure.Persistence.Implementations.Seeders
{
	public abstract class BaseDbSeeder(
		IUnitOfWork<MainDbContext> mainUnitOfWork,
		IOptions<AppSettings> appSettings,
		IEcomLogger logger) : IDbSeeder
	{
		protected readonly IUnitOfWork<MainDbContext> _mainUnitOfWork = mainUnitOfWork;
		protected readonly AppSettings _appSettings = appSettings.Value;
		protected readonly IEcomLogger _logger = logger;

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
				PropertyNameCaseInsensitive = true
			};
		}
	}
}
