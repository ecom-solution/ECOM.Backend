using ECOM.App.Interfaces.Loggings;
using ECOM.Domain.Entities.Main;
using ECOM.Domain.Interfaces.DataContracts;
using ECOM.Shared.Library.Consts;
using ECOM.Shared.Library.Models.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ECOM.Infrastructure.Implementations.Seeders.Main
{
	public class ApplicationRoleSeeder(
		ILog logger,
		IOptions<AppSettings> appSettings,
		[FromKeyedServices(DatabaseConstants.Main)] IUnitOfWork mainUnitOfWork) : DbSeeder(logger, appSettings, mainUnitOfWork)
	{
		public override int Priority => 1;

		public override async Task SeedAsync()
		{
			_logger.Information($"Seeding ApplicationRoles");

			var relativePath = Path.Combine("ECOM.Infrastructure.Database", "Main", "Seeds", "ApplicationRole.json");
			var roles = await ReadAsync<ApplicationRole>(relativePath);

			if (roles.Count > 0)
			{
				await _mainUnitOfWork.BulkUpsertAsync(roles, _appSettings.DbContext.Bulk.BatchSize, _appSettings.DbContext.Bulk.CmdTimeOutInMiliseconds);
				_logger.Information($"Seeded {roles.Count} ApplicationRoles");
			}
		}
	}
}
