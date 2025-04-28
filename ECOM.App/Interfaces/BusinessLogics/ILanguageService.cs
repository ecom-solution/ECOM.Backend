using ECOM.Shared.Library.Models.Dtos.Common;
using ECOM.Shared.Library.Models.Dtos.Modules.Language;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECOM.App.Interfaces.BusinessLogics
{
    public interface ILanguageService
    {
        /// <summary>
        /// Generates localization content (e.g., JSON or resource files)
        /// for a given array of language codes.
        /// </summary>
        /// <param name="languageCodes">An array of language codes (e.g., ["en", "vi", "fr"]).</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task GenerateLocalizationContentAsync(string[] languageCodes);

        /// <summary>
        /// Generates localization content for a specific language and uploads it to storage.
        /// </summary>
        /// <param name="languageCode">The language code for which to generate and upload content.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task GenerateLocalizationContentAndUploadToStorageAsync(string languageCode);

        /// <summary>
        /// Retrieves all available languages from the system.
        /// </summary>
        /// <returns>A list of <see cref="LanguageDto"/> objects representing the languages.</returns>
        Task<List<LanguageDto>> GetLanguagesAsync();

        /// <summary>
        /// Searches for languages based on the provided paginated request.
        /// </summary>
        /// <param name="request">The <see cref="PaginatedRequest{LanguageSearchModel}"/> containing search parameters.</param>
        /// <returns>A <see cref="PaginatedResponse{LanguageRecordModel}"/> containing the search results.</returns>
        Task<PaginatedResponse<LanguageRecordModel>> SearchAsync(PaginatedRequest<LanguageSearchModel> request);
    }
}