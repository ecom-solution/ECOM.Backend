using ECOM.App.Interfaces.Loggings;
using ECOM.Domain.Interfaces.DataContracts;
using ECOM.Domain.Interfaces.Seeders;
using ECOM.Shared.Library.Consts;
using ECOM.Shared.Library.Models.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ECOM.Infrastructure.Implementations.Seeders
{
	public class DbSeederModule(
		ILog logger,
		IOptions<AppSettings> appSettings,
		[FromKeyedServices(DatabaseConstants.Main)] IUnitOfWork mainUnitOfWork) : IDbSeederModule
	{
		protected readonly ILog _logger = logger;
		protected readonly AppSettings _appSettings = appSettings.Value;
		protected readonly IUnitOfWork _mainUnitOfWork = mainUnitOfWork;

		public async Task InitializeAsync(IEnumerable<IDbSeeder> seeders)
		{
			_logger.Information("Starting database seeding...");
			var sortedSeeders = seeders.OrderBy(s => s.Priority).ToList();

			await using var transaction = await _mainUnitOfWork.BeginTransactionAsync();

			foreach (var seeder in sortedSeeders)
			{
				_logger.Information($"Executing {seeder.GetType().Name}...");
				await seeder.SeedAsync();
			}

			await transaction.CommitAsync();
		}
	}
}
