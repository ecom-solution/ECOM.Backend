namespace ECOM.Shared.Library.Models.Dtos.Modules.Language
{
	public class LanguageDto
	{
		public string Code { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public bool IsDefault { get; set; }

		/// <summary>
		/// Optional avatar image URL for this language.
		/// </summary>
		public string? AvatarUrl { get; set; }
	}
}
