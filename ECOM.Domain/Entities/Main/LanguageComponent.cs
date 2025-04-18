namespace ECOM.Domain.Entities.Main
{
	public class LanguageComponent : BaseEntity
	{
		public LanguageComponent() { }

		public Guid? ParentId { get; set; }
		public string ComponentName { get; set; } = string.Empty;
		public string? Description { get; set; }

		public virtual LanguageComponent? Parent { get; set; }
		public virtual ICollection<LanguageComponent>? Children { get; set; }
		public virtual ICollection<LanguageKey>? LanguageKeys { get; set; }
	}
}
