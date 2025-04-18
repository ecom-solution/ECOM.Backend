namespace ECOM.Domain.Entities.Main
{
	public class LanguageKey : BaseEntity
    {
		public LanguageKey() { }

		public Guid LanguageComponentId { get; set; }
		public string Key { get; set; } = string.Empty;
		public string? Description { get; set; }

		public virtual LanguageComponent? LanguageComponent { get; set; }
		public virtual ICollection<LanguageTranslation>? LanguageTranslations { get; set; }
	}
}
