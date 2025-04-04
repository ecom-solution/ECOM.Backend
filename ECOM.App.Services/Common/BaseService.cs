using AutoMapper;
using ECOM.Domain.Interfaces.Repositories;
using ECOM.Infrastructure.Logging.Interfaces;
using ECOM.Infrastructure.Persistence.Main;
using ECOM.Infrastructure.Persistence.MainLogging;
using ECOM.Shared.Utilities.Settings;
using Microsoft.Extensions.Options;

namespace ECOM.App.Services.Common
{
	public abstract class BaseService(
		IMapper mapper,
		IEcomLogger logger,
		IOptions<AppSettings> appSettings,
		IUnitOfWork<MainDbContext> mainUnitOfWork,
		IUnitOfWork<MainLoggingDbContext> mainLoggingUnitOfWork)
	{
		protected readonly IMapper _mapper = mapper;
		protected readonly IEcomLogger _logger = logger;
		protected readonly AppSettings _appSettings = appSettings.Value;
		protected readonly IUnitOfWork<MainDbContext> _mainUnitOfWork = mainUnitOfWork;
		protected readonly IUnitOfWork<MainLoggingDbContext> _mainLoggingUnitOfWork = mainLoggingUnitOfWork;
	}
}
