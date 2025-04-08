using ECOM.App.Services.Common;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ECOM.App.Services.Extensions
{
	public static class DependencyExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			var assembly = Assembly.GetAssembly(typeof(BaseService));

			if (assembly != null)
			{
				var baseNamespace = typeof(BaseService).Namespace ?? string.Empty;

				var types = assembly.GetTypes()
									.Where(t => t.IsClass && !t.IsAbstract && t.Namespace != null && t.Namespace.StartsWith(baseNamespace))
									.ToList();

				foreach (var implementationType in types)
				{
					var interfaceType = implementationType.GetInterface($"I{implementationType.Name}");
					if (interfaceType != null)
					{
						services.AddScoped(interfaceType, implementationType);
					}
				}

				return services;
			}

			throw new InvalidOperationException("Cannot load assembly for BaseService.");
		}
	}
}
