using ECOM.App.Services.Interfaces;
using ECOM.Infrastructure.Logging.Interfaces;
using ECOM.Shared.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;

namespace ECOM.Presentation.API.Controllers
{
    [ApiController]
	[Route("api/[controller]")]
	public class LocalizationController(
		IEcomLogger logger,
		ILocalizationService localizationService) : ControllerBase
	{
		private readonly IEcomLogger _logger = logger;
		private readonly ILocalizationService _localizationService = localizationService;
		
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
