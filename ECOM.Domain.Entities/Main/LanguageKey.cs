namespace ECOM.Domain.Entities.Main
{
	public class LanguageKey : BaseEntity
    {
		public LanguageKey() { }

		public string Key { get; set; } = string.Empty;
		public string? Description { get; set; }

		public virtual ICollection<LanguageTranslation>? LanguageTranslations { get; set; }
	}
}
