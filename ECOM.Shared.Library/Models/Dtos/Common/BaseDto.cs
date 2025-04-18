using ECOM.Shared.Library.Functions.Helpers;

namespace ECOM.Shared.Library.Models.Dtos.Common
{
	public class BaseDto
	{
		public Guid Id { get; set; }
		public Guid? CreatedBy { get; set; }
		public DateTime CreatedAt_Utc { get; set; }

		public Guid? LastUpdatedBy { get; set; }
		public DateTime LastUpdatedAt_Utc { get; set; }

		protected BaseDto()
		{
			Id = CommonHelper.GenerateSequenceGuid();
			CreatedAt_Utc = DateTime.UtcNow;
			LastUpdatedAt_Utc = DateTime.UtcNow;
		}
	}
}
