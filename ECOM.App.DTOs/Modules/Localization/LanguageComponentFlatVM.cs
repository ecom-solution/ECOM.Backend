namespace ECOM.App.DTOs.Modules.Localization
{
	public class LanguageComponentFlatVM
	{
		public string Name { get; set; } = string.Empty;
		public Dictionary<string, string> Translations { get; set; } = new();
	}
}
