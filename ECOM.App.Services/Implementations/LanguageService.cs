using AutoMapper;
using ECOM.App.DTOs.Modules.Localization;
using ECOM.App.Services.Common;
using ECOM.App.Services.Interfaces;
using ECOM.Domain.Entities.Main;
using ECOM.Domain.Interfaces.Repositories;
using ECOM.Infrastructure.Logging.Interfaces;
using ECOM.Infrastructure.Persistence.Main;
using ECOM.Infrastructure.Persistence.MainLogging;
using ECOM.Shared.Utilities.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ECOM.App.Services.Implementations
{
	public class LanguageService(
		IMapper mapper,
		IEcomLogger logger,
		IOptions<AppSettings> appSettings,
		IUnitOfWork<MainDbContext> mainUnitOfWork,
		IUnitOfWork<MainLoggingDbContext> mainLoggingUnitOfWork)
		: BaseService(mapper, logger, appSettings, mainUnitOfWork, mainLoggingUnitOfWork), ILanguageService
	{
		public async Task<Dictionary<string, LanguageComponentFlatVM>> GetLanguageComponentChildrenFlatAsync(string languageCode, string parentComponentName)
		{
			// 1. Get LanguageId
			var languageId = await _mainUnitOfWork.Repository<Language>()
				.Where(l => l.Code == languageCode)
				.Select(l => l.Id)
				.FirstOrDefaultAsync();

			if (languageId == Guid.Empty)
				throw new Exception($"Language '{languageCode}' not found.");

			// 2. Get Parent Component Id
			var parentId = await _mainUnitOfWork.Repository<LanguageComponent>()
				.Where(c => c.ComponentName == parentComponentName)
				.Select(c => c.Id)
				.FirstOrDefaultAsync();

			if (parentId == Guid.Empty)
				throw new Exception($"Parent Component '{parentComponentName}' not found.");

			// 3. Query Children Components to memory
			var children = await _mainUnitOfWork.Repository<LanguageComponent>()
				.Where(c => c.ParentId == parentId)
				.ToListAsync(); // Query to memory

			var languageKeys = await _mainUnitOfWork.Repository<LanguageKey>()
				.Where(k => children.Select(c => c.Id).Contains(k.LanguageComponentId))
				.ToListAsync(); // Query all related keys

			var translations = await _mainUnitOfWork.Repository<LanguageTranslation>()
				.Where(t => t.LanguageId == languageId && languageKeys.Select(k => k.Id).Contains(t.LanguageKeyId))
				.ToListAsync(); // Query all related translations

			// 4. Build output
			var result = new Dictionary<string, LanguageComponentFlatVM>();

			foreach (var component in children)
			{
				var keysOfComponent = languageKeys.Where(k => k.LanguageComponentId == component.Id).ToList();

				var vm = new LanguageComponentFlatVM
				{
					Name = keysOfComponent
						.Where(k => k.Key == "name")
						.Select(k => translations.FirstOrDefault(t => t.LanguageKeyId == k.Id)?.Value ?? component.ComponentName)
						.FirstOrDefault() ?? component.ComponentName,

					Translations = keysOfComponent
						.Where(k => k.Key != "name")
						.ToDictionary(
							k => k.Key.ToLower(),
							k => translations.FirstOrDefault(t => t.LanguageKeyId == k.Id)?.Value ?? string.Empty
						)
				};

				result[component.ComponentName.ToLower()] = vm;
			}

			return result;
		}

	}
}
