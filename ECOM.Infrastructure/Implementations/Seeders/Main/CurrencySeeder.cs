using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECOM.App.Interfaces.Loggings;
using ECOM.Domain.Entities.Main;
using ECOM.Domain.Interfaces.DataContracts;
using ECOM.Shared.Library.Consts;
using ECOM.Shared.Library.Models.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ECOM.Infrastructure.Implementations.Seeders.Main
{
    public class CurrencySeeder(
        ILog logger,
        IOptions<AppSettings> appSettings,
        [FromKeyedServices(DatabaseConstants.Main)] IUnitOfWork mainUnitOfWork) : DbSeeder(logger, appSettings, mainUnitOfWork)
    {
        private const string SeedFileName = "Currency.json";
        private readonly string _seedFilePath = Path.Combine("ECOM.Infrastructure.Database", "Main", "Seeds", SeedFileName);
        private const string SeedName = "Currency";
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
                _logger.Information($"Currency seed data has changed or not yet seeded. Starting seeding...");

                var currencies = await ReadAsync<Currency>(_seedFilePath);

                if (currencies.Count > 0)
                {
                    _logger.Information($"Seeding Currency...");
                    await _mainUnitOfWork.BulkUpsertAsync(currencies, _appSettings.DbContext.Bulk.BatchSize, _appSettings.DbContext.Bulk.CmdTimeOutInMiliseconds);
                    _logger.Information($"Seeded {currencies.Count} currencies");

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
                    _logger.Information($"Currency seed state updated in the database.");
                }
                else
                {
                    _logger.Warning($"Currency seed file '{SeedFileName}' is empty.");
                }
            }
            else
            {
                _logger.Information($"Currency seed data has not changed since {existingSeedState.LastSeededAtUtc.ToLocalTime()} (last modified: {existingSeedState.LastModifiedAtUtc?.ToLocalTime()}). Skipping seeding.");
            }
        }
    }
}
