using ECOM.App.Interfaces.Loggings;
using ECOM.Domain.Interfaces.DataContracts;
using ECOM.Shared.Library.Consts;
using ECOM.Shared.Library.Models.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ECOM.App.Implementations.BusinessLogics.Common
{
	public abstract class BaseService
        (ILog logger,
		IOptions<AppSettings> appSettings,
		[FromKeyedServices(DatabaseConstants.Main)] IUnitOfWork mainUnitOfWork,
		[FromKeyedServices(DatabaseConstants.MainLogging)] IUnitOfWork loggingUnitOfWork)
    {
        protected readonly ILog _logger = logger;
		protected readonly AppSettings _appSettings = appSettings.Value;
		protected readonly IUnitOfWork _mainUnitOfWork = mainUnitOfWork;
		protected readonly IUnitOfWork _loggingUnitOfWork = loggingUnitOfWork;
    }
}
