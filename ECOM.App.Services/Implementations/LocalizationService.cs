using AutoMapper;
using ECOM.App.Services.Common;
using ECOM.App.Services.Interfaces;
using ECOM.Domain.Entities.Main;
using ECOM.Domain.Interfaces.Repositories;
using ECOM.Infrastructure.Logging.Interfaces;
using ECOM.Infrastructure.Persistence.Main;
using ECOM.Shared.Utilities.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ECOM.App.Services.Implementations
{
	public class LocalizationService(
		IMapper mapper,
		IEcomLogger logger,
		IOptions<AppSettings> appSettings,
		IUnitOfWork<MainDbContext> mainUnitOfWork)
		: BaseService(mapper, logger, appSettings, mainUnitOfWork), ILocalizationService
	{		
		public async Task<string> GenerateLocalizationContentAsync(string languageCode, string rootComponent)
		{
			var language = await _mainUnitOfWork.Repository<Language>()
											    .FirstOrDefaultAsync(l => l.Code == languageCode) ?? throw new Exception($"Language '{languageCode}' not found.");

			
			var root = await _mainUnitOfWork.Repository<LanguageComponent>()
											.FirstOrDefaultAsync(c => c.ComponentName == rootComponent) ?? throw new Exception($"Root component '{rootComponent}' not found.");

			

			var content = await BuildComponentTreeAsync(root.Id, language.Id);

			var result = new Dictionary<string, object>
			{
				{ root.ComponentName, content }
			};
			return JsonSerializer.Serialize(result, GetOptions());
		}

		private async Task<Dictionary<string, object>> BuildComponentTreeAsync(Guid componentId, Guid languageId)
		{
			var result = new Dictionary<string, object>();

			// 1. Get translations for this component
			var keys = await _mainUnitOfWork.Repository<LanguageKey>()
											.Where(k => k.LanguageComponentId == componentId)
											.ToListAsync();

			if (keys.Count != 0)
			{
				var keyIds = keys.Select(k => k.Id).ToList();

				var translations = await _mainUnitOfWork.Repository<LanguageTranslation>()
					.Where(t => keyIds.Contains(t.LanguageKeyId) && t.LanguageId == languageId)
					.ToListAsync();

				foreach (var key in keys)
				{
					var value = translations.FirstOrDefault(t => t.LanguageKeyId == key.Id)?.Value ?? "";
					result[key.Key] = value;
				}
			}

			// 2. Get children components
			var children = await _mainUnitOfWork.Repository<LanguageComponent>()
												.Where(c => c.ParentId == componentId)
												.ToListAsync();

			foreach (var child in children)
			{
				var childContent = await BuildComponentTreeAsync(child.Id, languageId);
				result[child.ComponentName] = childContent;
			}

			return result;
		}

		public static JsonSerializerOptions GetOptions()
		{
			return new JsonSerializerOptions { WriteIndented = true };
		}
	}
}
