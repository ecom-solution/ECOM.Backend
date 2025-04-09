namespace ECOM.App.DTOs.Modules.Localization
{
	public class LanguageComponentVM
	{
		public string ComponentName { get; set; } = string.Empty;
		public Dictionary<string, string> Translations { get; set; } = [];
		public List<LanguageComponentVM> Children { get; set; } = [];
	}
}
