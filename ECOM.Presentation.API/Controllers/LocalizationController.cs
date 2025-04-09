using ECOM.App.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECOM.Presentation.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class LocalizationController(ILanguageService languageService) : ControllerBase
	{
		private readonly ILanguageService _languageService = languageService;

		/// <summary>
		/// Get all children components and their translations by language code and parent component name
		/// </summary>
		/// <param name="languageCode">Language code (e.g., vi, en)</param>
		/// <param name="parentComponentName">Parent component name</param>
		/// <returns>Dictionary of child components</returns>
		[HttpGet("components/children")]
		public async Task<IActionResult> GetChildrenComponents(
			[FromQuery] string languageCode,
			[FromQuery] string parentComponentName)
		{
			if (string.IsNullOrWhiteSpace(languageCode))
				return BadRequest("LanguageCode is required.");

			if (string.IsNullOrWhiteSpace(parentComponentName))
				return BadRequest("ParentComponentName is required.");

			var rawResult = await _languageService.GetLanguageComponentChildrenFlatAsync(languageCode, parentComponentName);

			var flatResult = rawResult.ToDictionary(
				x => x.Key,
				x => {
					var inner = new Dictionary<string, string>();
					foreach (var kv in x.Value.Translations)
					{
						inner[kv.Key] = kv.Value;
					}
					return inner;
				});

			return Ok(flatResult);
		}
	}
}
