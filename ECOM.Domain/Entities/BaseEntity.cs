using ECOM.Shared.Library.Functions.Helpers;

namespace ECOM.Domain.Entities
{
	public abstract class BaseEntity
	{
		public Guid Id { get; set; } = CommonHelper.GenerateSequenceGuid();
	}
}
