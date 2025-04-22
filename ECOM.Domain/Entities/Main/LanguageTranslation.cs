namespace ECOM.Domain.Entities.Main
{
	public class LanguageTranslation
    {
		public LanguageTranslation() { }

		public LanguageTranslation(Guid languageId, Guid languageKeyId)
		{
			LanguageId = languageId;
			LanguageKeyId = languageKeyId;
		}

		public Guid LanguageId { get; set; }
		public Guid LanguageKeyId { get; set; }
		public string Value { get; set; } = string.Empty;

		public virtual Language? Language { get; set; }
		public virtual LanguageKey? LanguageKey { get; set; }
	}
}
