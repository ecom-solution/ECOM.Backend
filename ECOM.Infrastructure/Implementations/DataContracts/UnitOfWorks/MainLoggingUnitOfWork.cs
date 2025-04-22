using ECOM.Infrastructure.Database.MainLogging;

namespace ECOM.Infrastructure.Implementations.DataContracts.UnitOfWorks
{
	public class MainLoggingUnitOfWork(MainLoggingDbContext context) : UnitOfWork<MainLoggingDbContext>(context)
	{
	}
}
