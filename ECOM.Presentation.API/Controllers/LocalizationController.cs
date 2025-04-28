using ECOM.App.Interfaces.BusinessLogics;
using ECOM.App.Interfaces.Loggings;
using Microsoft.AspNetCore.Mvc;

namespace ECOM.Presentation.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class LocalizationController(
		ILog logger,
		ILanguageService languageService) : ControllerBase
	{
		private readonly ILog _logger = logger;
		private readonly ILanguageService _languageService = languageService;
		
		/// <summary>
		/// Generate full localization JSON from root component
		/// </summary>
		/// <param name="languageCode">Language code (e.g., vi, en)</param>
		/// <param name="rootComponent">Root component name</param>
		/// <returns>Localization content JSON</returns>
		[HttpPost("generate-translations")]
		public async Task<IActionResult> GenerateTranslationsAsync([FromQuery] string[] languageCodes)
		{
			await _languageService.GenerateLocalizationContentAsync(languageCodes);
			return Ok();
        }

	}
}
