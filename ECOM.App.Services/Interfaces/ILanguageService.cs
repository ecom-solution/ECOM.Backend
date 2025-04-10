using ECOM.App.DTOs.Modules.Localization;

namespace ECOM.App.Services.Interfaces
{
	public interface ILanguageService
	{
		Task<string> GenerateLocalizationContentAsync(string languageCode, string rootComponent);
	}
}
