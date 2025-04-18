using ECOM.App.Interfaces.BusinessLogics;
using ECOM.App.Interfaces.Loggings;
using ECOM.Shared.Library.Consts;
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
		[HttpGet("get-translations")]
		public async Task<IActionResult> GetTranslationsAsync(
			[FromQuery] string languageCode,
			[FromQuery] string rootComponent)
		{
			if (string.IsNullOrWhiteSpace(languageCode))
				return BadRequest("LanguageCode is required.");

			if (string.IsNullOrWhiteSpace(rootComponent))
				return BadRequest("RootComponent is required.");

			var jsonContent = await _languageService.GenerateLocalizationContentAsync(languageCode, rootComponent);

			return Content(jsonContent, FileContentType.Json);
		}

		[HttpGet("test-sendmail")]
		public async Task<IActionResult> TestSendMailAsync()
		{
			await _languageService.TestSendMailAsync();
			return Ok();
		}
	}
}
