using ECOM.Domain.Entities.Main;
using ECOM.Domain.Interfaces.Repositories;
using ECOM.Infrastructure.Logging.Interfaces;
using ECOM.Infrastructure.Persistence.Extensions;
using ECOM.Infrastructure.Persistence.Main;
using ECOM.Shared.Utilities.Settings;
using Microsoft.Extensions.Options;

namespace ECOM.Infrastructure.Persistence.Implementations.Seeders.Main
{
	public class LanguageSeeder(
		IUnitOfWork<MainDbContext> mainUnitOfWork,
		IOptions<AppSettings> appSettings,
		IEcomLogger logger) : BaseDbSeeder(mainUnitOfWork, appSettings, logger)
	{
		public override int Priority => 1;
		public override async Task SeedAsync()
		{
			_logger.Information($"Seeding Language...");

			var relativePath = Path.Combine("ECOM.Infrastructure.Persistence.Main", "Seeds", "Language.json");
			var languages = await ReadAsync<Language>(relativePath);

			if (languages.Count > 0)
			{
				await _mainUnitOfWork.GetContext().BulkUpsertAsync(languages, _appSettings.DbContext.Bulk.BatchSize, _appSettings.DbContext.Bulk.CmdTimeOutInMiliseconds);
				_logger.Information($"Seeded {languages.Count} Languages");
			}
		}
	}
}
