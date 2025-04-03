namespace ECOM.Domain.Entities.Main
{
	public class Language : BaseEntity
    {
		public Language() { }

		public string Code { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public Guid? AvatarId { get; set; }

		public virtual FileEntity? Avatar { get; set; }
		public virtual ICollection<LanguageTranslation>? LanguageTranslations { get; set; }
	}
}
