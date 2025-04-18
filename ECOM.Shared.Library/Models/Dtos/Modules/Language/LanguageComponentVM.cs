using ECOM.Shared.Library.Enums.Entity;

namespace ECOM.Shared.Library.Models.Dtos.Modules.Language
{
	public class LanguageComponentVM
	{
		public Guid Id { get; set; }
		public Guid? ParentId { get; set; }
		public string ComponentName { get; set; } = string.Empty;
		public string? Description { get; set; }
	}

	public class LanguageComponentFilterModel
	{
		public LanguageComponentType Type { get; set; }
	}

	public class LanguageComponentRecordModel : LanguageComponentVM
	{

	}
}
