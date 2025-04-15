using ECOM.Shared.Library.Models.Dtos.Common;
using ECOM.Shared.Library.Models.Dtos.Modules.Language;

namespace ECOM.App.Services.Interfaces
{
	public interface ILanguageComponentService
    {
        Task<PaginatedResponse<LanguageComponentRecordModel>> Filter(PaginatedRequest<LanguageComponentFilterModel> request);
    }
}
