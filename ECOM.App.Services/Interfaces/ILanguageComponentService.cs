using ECOM.App.DTOs.Common;
using ECOM.App.DTOs.Modules.Localization;

namespace ECOM.App.Services.Interfaces
{
	public interface ILanguageComponentService
	{
		Task<PaginatedResponse<LanguageComponentRecordModel>> Filter(PaginatedRequest<LanguageComponentFilterModel> request);
	}
}
