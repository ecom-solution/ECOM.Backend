namespace ECOM.Domain.Interfaces.Seeders
{
	public interface IDbSeederModule
	{
		Task InitializeAsync(IEnumerable<IDbSeeder> seeders);
	}
}
