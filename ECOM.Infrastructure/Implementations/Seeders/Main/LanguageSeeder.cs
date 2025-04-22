using ECOM.App.Interfaces.Loggings;
using ECOM.Domain.Entities.Main;
using ECOM.Domain.Interfaces.DataContracts;
using ECOM.Shared.Library.Consts;
using ECOM.Shared.Library.Models.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ECOM.Infrastructure.Implementations.Seeders.Main
{
	public class LanguageSeeder(
		ILog logger,
		IOptions<AppSettings> appSettings,
		[FromKeyedServices(DatabaseConstants.Main)] IUnitOfWork mainUnitOfWork) : DbSeeder(logger, appSettings, mainUnitOfWork)
	{
        private const string SeedFileName = "Language.json";
        private readonly string _seedFilePath = Path.Combine("ECOM.Infrastructure.Database", "Main", "Seeds", SeedFileName);
        private const string SeedName = "Language";
		private const int PartialHashBytes = 4096;

        public override int Priority => 1;
		public override async Task SeedAsync()
		{
            var currentHash = await CalculatePartialFileHashAsync(_seedFilePath, PartialHashBytes);
            var lastModified = File.GetLastWriteTimeUtc(_seedFilePath);

            var seedStateRepository = _mainUnitOfWork.Repository<SeedState>();
            var existingSeedState = await seedStateRepository.FirstOrDefaultAsync(s => s.SeedName == SeedName);

            if (existingSeedState == null || existingSeedState.CurrentHash != currentHash || existingSeedState.LastModifiedAtUtc < lastModified)
            {
                _logger.Information($"Language seed data has changed or not yet seeded. Starting seeding...");

                var languages = await ReadAsync<Language>(_seedFilePath);

                if (languages.Count > 0)
                {
                    _logger.Information($"Seeding Language...");
                    await _mainUnitOfWork.BulkUpsertAsync(languages, _appSettings.DbContext.Bulk.BatchSize, _appSettings.DbContext.Bulk.CmdTimeOutInMiliseconds);
                    _logger.Information($"Seeded {languages.Count} Languages");

                    // Update seedstate in database
                    if (existingSeedState == null)
                    {
                        await seedStateRepository.InsertAsync(new SeedState
                        {
                            SeedName = SeedName,
                            CurrentHash = currentHash,
                            LastModifiedAtUtc = lastModified,
                            LastSeededAtUtc = DateTime.UtcNow
                        });
                    }
                    else
                    {
                        existingSeedState.CurrentHash = currentHash;
                        existingSeedState.LastModifiedAtUtc = lastModified;
                        existingSeedState.LastSeededAtUtc = DateTime.UtcNow;
                        seedStateRepository.Update(existingSeedState);
                    }
                    await _mainUnitOfWork.SaveChangesAsync();
                    _logger.Information($"Language seed state updated in the database.");
                }
                else
                {
                    _logger.Warning($"Language seed file '{SeedFileName}' is empty.");
                }
            }
            else
            {
                _logger.Information($"Language seed data has not changed since {existingSeedState.LastSeededAtUtc.ToLocalTime()} (last modified: {existingSeedState.LastModifiedAtUtc?.ToLocalTime()}). Skipping seeding.");
            }
        }    
	}
}
