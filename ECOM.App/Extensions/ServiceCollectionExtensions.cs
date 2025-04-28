using ECOM.App.Implementations.BusinessLogics.Common;
using ECOM.App.Implementations.Users;
using ECOM.App.Interfaces.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ECOM.App.Extensions
{
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Automatically registers all classes that inherit from BaseService
		/// and implement an interface named I{ClassName} as scoped services.
		/// </summary>
		/// <param name="services">The DI container to add services to.</param>
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();

			//-----------------------------------------------------------------------

            var baseServiceType = typeof(BaseService);

			// Load the assembly where BaseService is defined
			var assembly = baseServiceType.Assembly;

			var serviceTypes = assembly.GetTypes()
				.Where(type => type is { IsClass: true, IsAbstract: false }
					&& type.BaseType != null
					&& baseServiceType.IsAssignableFrom(type.BaseType));

			foreach (var implementation in serviceTypes)
			{
				// Try to match interface named "I{ClassName}"
				var serviceInterface = implementation.GetInterface($"I{implementation.Name}");

				if (serviceInterface != null)
				{
					services.AddScoped(serviceInterface, implementation);
				}
			}

            return services;
		}
	}
}
