using ECOM.App.Implementations.BusinessLogics.Common;
using ECOM.App.Interfaces.BusinessLogics;
using ECOM.App.Interfaces.Loggings;
using ECOM.Domain.Entities.Main;
using ECOM.Domain.Interfaces.DataContracts;
using ECOM.Shared.Library.Models.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ECOM.App.Implementations.BusinessLogics
{
	public class LanguageService(
		ILog logger,
		IOptions<AppSettings> appSettings,
		[FromKeyedServices("Main")] IUnitOfWork mainUnitOfWork,
		[FromKeyedServices("MainLogging")] IUnitOfWork loggingUnitOfWork)
		: BaseService(logger, appSettings, mainUnitOfWork, loggingUnitOfWork), ILanguageService
	{
		public async Task<string> GenerateLocalizationContentAsync(string languageCode, string rootComponent)
		{
			var language = await _mainUnitOfWork.Repository<Language>()
												.FirstOrDefaultAsync(l => l.Code == languageCode) 
												?? throw new Exception($"Language '{languageCode}' not found.");


			var root = await _mainUnitOfWork.Repository<LanguageComponent>()
											.FirstOrDefaultAsync(c => c.ComponentName == rootComponent) 
											?? throw new Exception($"Root component '{rootComponent}' not found.");



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
			var keysQuery = _mainUnitOfWork.Repository<LanguageKey>().Where(k => k.LanguageComponentId == componentId);
			var keys = await _mainUnitOfWork.Repository<LanguageKey>().ToListAsync(keysQuery);

			if (keys.Count != 0)
			{
				var keyIds = keys.Select(k => k.Id).ToList();

				var translationsQuery = _mainUnitOfWork.Repository<LanguageTranslation>()
													   .Where(t => keyIds.Contains(t.LanguageKeyId) && t.LanguageId == languageId);
				var translations = await _mainUnitOfWork.Repository<LanguageTranslation>().ToListAsync(translationsQuery);

				foreach (var key in keys)
				{
					var value = translations.FirstOrDefault(t => t.LanguageKeyId == key.Id)?.Value ?? "";
					result[key.Key] = value;
				}
			}

			// 2. Get children components
			var childrenQuery = _mainUnitOfWork.Repository<LanguageComponent>().Where(c => c.ParentId == componentId);
			var children = await _mainUnitOfWork.Repository<LanguageComponent>().ToListAsync(childrenQuery);

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
