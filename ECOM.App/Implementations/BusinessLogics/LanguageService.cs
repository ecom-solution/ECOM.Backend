using ECOM.App.Extensions;
using ECOM.App.Implementations.BusinessLogics.Common;
using ECOM.App.Interfaces.BusinessLogics;
using ECOM.App.Interfaces.Loggings;
using ECOM.App.Interfaces.Users;
using ECOM.Domain.Entities.Main;
using ECOM.Domain.Interfaces.DataContracts;
using ECOM.Domain.Interfaces.Messagings;
using ECOM.Domain.Interfaces.Storages;
using ECOM.Shared.Library.Consts;
using ECOM.Shared.Library.Models.Dtos.Common;
using ECOM.Shared.Library.Models.Dtos.Modules.Language;
using ECOM.Shared.Library.Models.Externals.RabbitMQ;
using ECOM.Shared.Library.Models.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ECOM.App.Implementations.BusinessLogics
{
	public class LanguageService(
		ILog logger,
		IOptions<AppSettings> appSettings,
		[FromKeyedServices("Main")] IUnitOfWork mainUnitOfWork,
        ICurrentUserAccessor currentUserAccessor,
        IPublisher publisher,
        IStorage storage)
		: BaseService(logger, appSettings, mainUnitOfWork, currentUserAccessor), ILanguageService
	{
		private readonly IPublisher _publisher = publisher;
        private readonly IStorage _storage = storage;

        public async Task GenerateLocalizationContentAsync(string[] languageCodes)
		{
            await _publisher.PublishAsync(new LocalizationContentGenerateMessage()
            {
                LanguageCodes = languageCodes
            }, _appSettings.RabbitMQ.LocalizationContentGenerateQueue);

        }

        public async Task GenerateLocalizationContentAndUploadToStorageAsync(string languageCode)
        {
            _logger.Information($"Starting to generate and upload localization content for language: '{languageCode}'."); // Log start

            try
            {
                // Retrieve the language entity based on the provided language code
                var language = await _mainUnitOfWork.Repository<Language>()
                                                    .FirstOrDefaultAsync(l => l.Code.ToLower() == languageCode.ToLower());

                // Check if the language was found
                if (language == null)
                {
                    _logger.Warning($"Language with code '{languageCode}' not found.");

                    // Publish a notification to the current user indicating the language was not found
                    //await _publisher.PublishAsync(new NotificationMessage()
                    //{
                    //    TargetUserIds = [_currentUserAccessor.Id],
                    //    Title = "",
                    //    Content = $"Language '{languageCode}' not found.",
                    //    CreatedAt_Utc = DateTime.UtcNow,
                    //}, _appSettings.RabbitMQ.NotifyQueue);

                    return;
                }

                _logger.Debug($"Retrieved language entity for code: '{languageCode}'.");

                // Query for language translations for the specific language, including the language key
                var translationsQuery = _mainUnitOfWork.Repository<LanguageTranslation>()
                                                       .Include(lt => lt.LanguageKey)
                                                       .Where(lt => lt.LanguageId == language.Id);

                // Retrieve the list of translations
                var translations = await _mainUnitOfWork.Repository<LanguageTranslation>().ToListAsync(translationsQuery, x => x);

                _logger.Debug($"Retrieved {translations.Count} translations for language: '{languageCode}'.");

                // Convert the translations to a dictionary where the key is the LanguageKey and the value is the translation
                var localizationData = translations.OrderBy(x => x.LanguageKey?.Key).ToDictionary(
                    lt => lt.LanguageKey?.Key ?? string.Empty,
                    lt => lt.Value ?? string.Empty);

                // Serialize the localization data to JSON format
                var jsonContent = JsonSerializer.Serialize(localizationData, JsonSerializerOptions);
                var jsonDataBytes = System.Text.Encoding.UTF8.GetBytes(jsonContent);

                _logger.Debug($"Localization content for '{languageCode}' generated. JSON size: {jsonDataBytes.Length} bytes.");

                // Upload the JSON content to the storage service (MinIO in this case)
                var uploadResponse = await _storage.UploadAsync(
                    bucketName: MinIOConstants.BucketName.Localizations,
                    objectName: $"{MinIOConstants.ObjectName.Translation}-{languageCode}.json",
                    jsonDataBytes,
                    FileContentType.Json);

                _logger.Information($"Localization content for '{languageCode}' uploaded successfully. File URL: '{uploadResponse.FileUrl}'.");

                // Publish a notification to the current user indicating the successful upload
                //await _publisher.PublishAsync(new NotificationMessage()
                //{
                //    TargetUserIds = [_currentUserAccessor.Id],
                //    Title = "Localization Uploaded",
                //    Content = $"Localization content for '{languageCode}' has been successfully uploaded to storage.",
                //    CreatedAt_Utc = DateTime.UtcNow,
                //    Links =
                //    [
                //        new NotificationLinkMessage()
                //        {
                //            Url = uploadResponse.FileUrl,
                //        }
                //    ]
                //}, _appSettings.RabbitMQ.NotifyQueue);
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred while generating and uploading localization content for language: '{languageCode}'.", ex);
                //await _publisher.PublishAsync(new NotificationMessage()
                //{
                //    TargetUserIds = [_currentUserAccessor.Id],
                //    Title = "Localization Upload Failed",
                //    Content = $"Failed to generate and upload localization content for '{languageCode}'. Error: {ex.Message}",
                //    CreatedAt_Utc = DateTime.UtcNow,
                //}, _appSettings.RabbitMQ.NotifyQueue);
                throw;
            }
        }

        public async Task<List<LanguageDto>> GetLanguagesAsync()
		{
			var defaultLanguageCode = ApplicationConstants.DefaultLanguage;

			var query = _mainUnitOfWork.Repository<Language>()
				.Include(x => x.Avatar)
				.OrderBy(x => x.Name);

			var result = await _mainUnitOfWork.Repository<Language>().ToListAsync(query, x => new LanguageDto
            {
                Code = x.Code,
                Name = x.Name,
                IsDefault = x.IsDefault,
                AvatarUrl = x.Avatar != null ? x.Avatar.FileUrl : null
            });

            var defaultLanguage = result.FirstOrDefault(x => x.IsDefault);
			if (defaultLanguage == null)
			{
				defaultLanguage = result.FirstOrDefault(x => x.Code == defaultLanguageCode);
				if (defaultLanguage != null)
					defaultLanguage.IsDefault = true;
            }	

            return result;
		}

		public async Task<PaginatedResponse<LanguageRecordModel>> SearchAsync(PaginatedRequest<LanguageSearchModel> request)
		{
			var languageRepository = _mainUnitOfWork.Repository<Language>();
            var query = languageRepository.Include(x => x.Avatar);

            // Apply GlobalFilter
            if (!string.IsNullOrEmpty(request.GlobalFilter?.Key))
            {
                query = query.Where(x =>
									x.Code.Contains(request.GlobalFilter.Key, StringComparison.CurrentCultureIgnoreCase) ||
									x.Name.Contains(request.GlobalFilter.Key, StringComparison.CurrentCultureIgnoreCase));
            }

            // Apply Filtering & Sorting By Columns
            query = query.ApplyFilteringByColumns(request.Columns)
						 .ApplySortingByColumns(request.Columns);

            var totalRecords = await languageRepository.CountAsync(query, x => x);
            var totalPages = (int)Math.Ceiling(totalRecords / (double)request.PageSize);

            // Apply Pagination
            var paginatedQuery = query.ApplyPagination(request.PageNumber, request.PageSize);

			var records = await languageRepository.ToListAsync(paginatedQuery, x => new LanguageRecordModel()
			{
				Id = x.Id,
				Code = x.Code,
                Name = x.Name,
                IsDefault = x.IsDefault,
                AvatarUrl = x.Avatar != null ? x.Avatar.FileUrl : null
            });

            return new PaginatedResponse<LanguageRecordModel>
            {
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Records = records
            };
        }
               
    }
}
