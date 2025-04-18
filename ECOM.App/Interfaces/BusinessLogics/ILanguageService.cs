using ECOM.Shared.Library.Models.Dtos.Modules.Language;

namespace ECOM.App.Interfaces.BusinessLogics
{
	public interface ILanguageService
	{
		/// <summary>
		/// Generates a localization content string (e.g., JSON or resource) 
		/// for a given language and root component.
		/// </summary>
		/// <param name="languageCode">The language code (e.g., "en", "vi", "fr").</param>
		/// <param name="rootComponent">The root component or namespace for which localization is generated.</param>
		/// <returns>A localized content string, often used to populate client-side i18n or resources.</returns>
		Task<string> GenerateLocalizationContentAsync(string languageCode, string rootComponent);

		/// <summary>
		/// Retrieves all available languages from the system, including their metadata such as code, name,
		/// avatar URL, and whether each one is the default language.
		/// </summary>
		/// <returns>A list of <see cref="LanguageDto"/> objects representing all supported languages.</returns>
		Task<List<LanguageDto>> GetLanguagesAsync();

		Task TestSendMailAsync();
	}
}
