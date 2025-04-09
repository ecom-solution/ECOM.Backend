using ECOM.Domain.Interfaces.Repositories;
using ECOM.Domain.Interfaces.Seeders;
using ECOM.Infrastructure.Logging.Interfaces;
using ECOM.Infrastructure.Persistence.Main;
using ECOM.Shared.Utilities.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ECOM.Infrastructure.Persistence.Implementations.Seeders
{
	public class DbSeederModule(
		IUnitOfWork<MainDbContext> mainUnitOfWork,
		IOptions<AppSettings> appSettings,
		IEcomLogger logger) : IDbSeederModule
	{
		protected readonly IUnitOfWork<MainDbContext> _mainUnitOfWork = mainUnitOfWork;
		protected readonly AppSettings _appSettings = appSettings.Value;
		protected readonly IEcomLogger _logger = logger;

		public async Task InitializeAsync(IEnumerable<IDbSeeder> seeders)
		{
			_logger.Information("Starting database seeding...");

			var sortedSeeders = seeders.OrderBy(s => s.Priority).ToList();

			await using var transaction = await _mainUnitOfWork.GetContext().Database.BeginTransactionAsync();

			foreach (var seeder in sortedSeeders)
			{
				_logger.Information($"Executing {seeder.GetType().Name}...");
				await seeder.SeedAsync();
			}

			await transaction.CommitAsync();
		}

	}
}
