namespace ECOM.Domain.Entities
{
	public abstract class AuditableEntity : BaseEntity
	{
		public Guid? CreatedBy { get; set; }
		public DateTime CreatedAt_Utc { get; set; } = DateTime.UtcNow;

		public Guid? LastUpdatedBy { get; set; }
		public DateTime LastUpdatedAt_Utc { get; set; } = DateTime.UtcNow;
	}
}
