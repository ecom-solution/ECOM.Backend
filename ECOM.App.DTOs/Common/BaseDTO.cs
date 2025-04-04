using ECOM.Shared.Utilities.Helpers;

namespace ECOM.App.DTOs.Common
{
	public abstract class BaseDTO
	{
		public Guid Id { get; set; }
		public Guid? CreatedBy { get; set; }
		public DateTime CreatedAt_Utc { get; set; }

		public Guid? LastUpdatedBy { get; set; }
		public DateTime LastUpdatedAt_Utc { get; set; }

		protected BaseDTO()
		{
			Id = GuidHelper.GenerateSequenceGuid();
			CreatedAt_Utc = DateTime.UtcNow;
			LastUpdatedAt_Utc = DateTime.UtcNow;
		}
	}
}
