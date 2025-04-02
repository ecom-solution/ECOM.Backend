using ECOM.Shared.Utilities.Helpers;

namespace ECOM.Domain.Entities
{
	public abstract class BaseEntity
	{
		public Guid Id { get; set; } = GuidHelper.GenerateSequenceGuid();
	}
}
