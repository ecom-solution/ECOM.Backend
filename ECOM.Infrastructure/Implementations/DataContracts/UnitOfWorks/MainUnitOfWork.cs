using ECOM.Infrastructure.Database.Main;

namespace ECOM.Infrastructure.Implementations.DataContracts.UnitOfWorks
{
	public class MainUnitOfWork(MainDbContext context) : UnitOfWork<MainDbContext>(context)
	{
	}
}
