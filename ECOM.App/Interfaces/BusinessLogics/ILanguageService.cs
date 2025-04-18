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
	}
}
