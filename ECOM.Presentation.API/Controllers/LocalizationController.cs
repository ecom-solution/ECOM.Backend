using ECOM.App.Services.Interfaces;
using ECOM.Infrastructure.External.Services.Interfaces;
using ECOM.Infrastructure.Logging.Interfaces;
using ECOM.Shared.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;

namespace ECOM.Presentation.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class LocalizationController(
		IEcomLogger logger,
		IFileEntityService fileEntityService,
		IMinIOStorageService minIOStorageService,
		ILocalizationService localizationService) : ControllerBase
	{
		private readonly IEcomLogger _logger = logger;
		private readonly IFileEntityService _fileEntityService = fileEntityService;
		private readonly IMinIOStorageService _minIOStorageService = minIOStorageService;
		private readonly ILocalizationService _localizationService = localizationService;

		/// <summary>
		/// Generate full localization JSON from root component and upload to storage
		/// </summary>
		/// <param name="languageCode">Language code (e.g., vi, en)</param>
		/// <param name="rootComponent">Root component name</param>
		/// <returns>Localization content JSON</returns>
		[HttpGet("update-translations")]
		public async Task<IActionResult> UpdateTranslationsAsync(
			[FromQuery] string languageCode,
			[FromQuery] string rootComponent)
		{
			if (string.IsNullOrWhiteSpace(languageCode))
				return BadRequest("LanguageCode is required.");

			if (string.IsNullOrWhiteSpace(rootComponent))
				return BadRequest("RootComponent is required.");

			var jsonContent = await _localizationService.GenerateLocalizationContentAsync(languageCode, rootComponent);

			var uploadFile = await _minIOStorageService.UploadAsync(
				MinIOStorageConstants.BucketName.Localizations,
				$"{MinIOStorageConstants.ObjectName.Translation}_{rootComponent}.{languageCode}.json",
				System.Text.Encoding.UTF8.GetBytes(jsonContent),
				FileContentType.Json);

			await _fileEntityService.InsertFileEntityAsync(uploadFile);

			return Ok(uploadFile);
		}

		/// <summary>
		/// Generate full localization JSON from root component
		/// </summary>
		/// <param name="languageCode">Language code (e.g., vi, en)</param>
		/// <param name="rootComponent">Root component name</param>
		/// <returns>Localization content JSON</returns>
		[HttpGet("get-translations")]
		public async Task<IActionResult> GetTranslationsAsync(
			[FromQuery] string languageCode,
			[FromQuery] string rootComponent)
		{
			if (string.IsNullOrWhiteSpace(languageCode))
				return BadRequest("LanguageCode is required.");

			if (string.IsNullOrWhiteSpace(rootComponent))
				return BadRequest("RootComponent is required.");

			var jsonContent = await _localizationService.GenerateLocalizationContentAsync(languageCode, rootComponent);

			return Content(jsonContent, FileContentType.Json);
		}
	}
}
