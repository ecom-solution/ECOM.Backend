using ECOM.App.DTOs.Modules.Localization;

namespace ECOM.App.Services.Interfaces
{
	public interface ILanguageService
	{
		Task<Dictionary<string, LanguageComponentFlatVM>> GetLanguageComponentChildrenFlatAsync(string languageCode, string parentComponentName);
	}
}
