namespace ECOM.Domain.Interfaces.Seeders
{
	public interface IDbSeeder
	{
		/// <summary>
		/// Lower value = run earlier
		/// </summary>
		int Priority { get; }

		Task SeedAsync();
	}
}
