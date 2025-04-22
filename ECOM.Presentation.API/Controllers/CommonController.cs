using ECOM.App.Interfaces.BusinessLogics;
using ECOM.Shared.Library.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECOM.Presentation.API.Controllers
{
	[ApiController]
	[AllowAnonymous]
	[Route("api/[controller]")]
	public class CommonController(
		ILanguageService languageService,
        ICurrencyService currencyService) : ControllerBase
	{
		private readonly ILanguageService _languageService = languageService;
		private readonly ICurrencyService _currencyService = currencyService;

        /// <summary>
        /// Returns a list of available time zones with their IDs and display names.
        /// Marks the default time zone based on ApplicationConstants.
        /// </summary>
        /// <returns>List of time zones with TimeZoneId, TimeZoneName, and IsDefault flag.</returns>
        [HttpGet("timezones")]
		public IActionResult GetTimeZones()
		{
			var defaultTz = ApplicationConstants.DefaultTimeZoneId;

			var timeZones = TimeZoneInfo.GetSystemTimeZones()
				.Select(tz => new
				{
					TimeZoneId = tz.Id,
					TimeZoneName = tz.DisplayName,
					ShortName = $"(UTC{tz.BaseUtcOffset:hh\\:mm})", // 👉 Format as (UTC+07:00) or (UTC-02:00)
					IsDefault = tz.Id == defaultTz
				})
				.OrderByDescending(tz => tz.IsDefault)
				.ThenBy(tz => tz.TimeZoneName)
				.ToList();

			return Ok(timeZones);
		}

        /// <summary>
        /// Retrieves a list of currencies, including their ISO codes, symbols, and display names.
        /// This endpoint also identifies and marks the default currency based on the configuration in ApplicationConstants.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the HTTP response.
        /// Upon success, it returns an <see cref="OkObjectResult"/> containing a list of <see cref="CurrencyDto"/> objects.
        /// Each <see cref="CurrencyDto"/> includes the currency's code, name, symbol, and a boolean flag indicating if it is the default currency.
        /// </returns>
        [HttpGet("currencies")]
		public async Task<IActionResult> GetCurrencies()
		{
			var result = await _currencyService.GetCurrenciesAsync();
            return Ok(result);
        }

		/// <summary>
		/// Gets all supported languages with basic metadata.
		/// </summary>
		/// <returns>List of supported languages.</returns>
		[HttpGet("languages")]
		public async Task<IActionResult> GetLanguages()
		{
			var result = await _languageService.GetLanguagesAsync();
			return Ok(result);
		}
	}
}
