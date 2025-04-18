using ECOM.App.Interfaces.BusinessLogics;
using ECOM.Shared.Library.Consts;
using ECOM.Shared.Library.Models.Dtos.Modules.Language;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace ECOM.Presentation.API.Controllers
{
	[ApiController]
	[AllowAnonymous]
	[Route("api/[controller]")]
	public class CommonController(ILanguageService languageService) : ControllerBase
	{
		private readonly ILanguageService _languageService = languageService;

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
					TimeZoneId = tz.Id,                // The system identifier of the time zone
					TimeZoneName = tz.DisplayName,     // The human-readable name (e.g., "(UTC+07:00) Bangkok, Hanoi, Jakarta")
					IsDefault = tz.Id == defaultTz     // Indicates whether this is the default time zone
				})
				.ToList();

			return Ok(timeZones);
		}

		/// <summary>
		/// Returns a list of currencies with ISO codes, symbols, and display names.
		/// Marks the default currency based on ApplicationConstants.
		/// </summary>
		/// <returns>List of currencies with CurrencyCode, CurrencySymbol, DisplayName, and IsDefault flag.</returns>
		[HttpGet("currencies")]
		public IActionResult GetCurrencies()
		{
			var defaultCurrency = ApplicationConstants.DefaultCurrency;

			var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

			var currencyList = cultures
				.Select(culture =>
				{
					try
					{
						if (string.IsNullOrWhiteSpace(culture.Name))
							return null;

						var region = new RegionInfo(culture.Name);
						return new
						{
							CurrencyCode = region.ISOCurrencySymbol,   // ISO 4217 code (e.g., USD, VND)
							CurrencySymbol = region.CurrencySymbol,    // Symbol (e.g., $, ₫)
							DisplayName = region.CurrencyEnglishName,  // English name (e.g., Vietnamese Dong)
							IsDefault = region.ISOCurrencySymbol == defaultCurrency
						};
					}
					catch
					{
						return null;
					}
				})
				.Where(x => x != null)
				.DistinctBy(x => x!.CurrencyCode)
				.OrderBy(x => x!.CurrencyCode)
				.ToList();

			return Ok(currencyList);
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
