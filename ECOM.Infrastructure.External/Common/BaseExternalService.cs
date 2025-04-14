using ECOM.Infrastructure.Logging.Interfaces;
using ECOM.Shared.Utilities.Settings;
using Microsoft.Extensions.Options;

namespace ECOM.Infrastructure.External.Common
{
	public abstract class BaseExternalService(IEcomLogger logger, IOptions<AppSettings> appSettings)
	{
		protected readonly IEcomLogger _logger = logger;
		protected readonly AppSettings _appSettings = appSettings.Value;
	}
}
