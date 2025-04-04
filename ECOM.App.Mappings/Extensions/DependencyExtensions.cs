using Microsoft.Extensions.DependencyInjection;

namespace ECOM.App.Mappings.Extensions
{
	public static class DependencyExtensions
	{
		public static IServiceCollection AddMappingModule(this IServiceCollection services)
		{
			services.AddAutoMapper(typeof(DependencyExtensions).Assembly);
			return services;
		}
	}
}
