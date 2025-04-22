namespace ECOM.Domain.Entities.Main
{
	public class LanguageTranslationEntity
    {
		public LanguageTranslationEntity() { }
		public LanguageTranslationEntity(Guid languageId, string entityName, Guid entityId)
		{
			LanguageId = languageId;
			EntityName = entityName;
			EntityId = entityId;
		}

		public Guid LanguageId { get; set; }
		public string EntityName { get; set; } = string.Empty;
		public Guid EntityId { get; set; }
		public string FieldName { get; set; } = string.Empty;
		public string Value { get; set; } = string.Empty;

		public virtual Language? Language { get; set; }
	}
}
