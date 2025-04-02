using ECOM.Shared.Utilities.Helpers;

namespace ECOM.Domain.Entities
{
	public abstract class BaseEntity
	{
		public Guid Id { get; set; } = GuidHelper.GenerateSequenceGuid();

		public Guid? CreatedBy { get; set; }
		public DateTime CreatedAt_Utc { get; set; } = DateTime.UtcNow;

		public Guid? LastUpdatedBy { get; set; }
		public DateTime LastUpdatedAt_Utc { get; set; } = DateTime.UtcNow;
	}
}
