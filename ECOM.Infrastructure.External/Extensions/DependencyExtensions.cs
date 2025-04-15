using ECOM.Infrastructure.External.Common;
using ECOM.Shared.Library.Models.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
using System.Reflection;

namespace ECOM.Infrastructure.External.Extensions
{
	public static class DependencyExtensions
	{
		public static IServiceCollection AddExternalModule(this IServiceCollection services)
		{
			services.AddMinIOStorage();

			var assembly = Assembly.GetAssembly(typeof(BaseExternalService)) ?? throw new InvalidOperationException("Cannot load assembly for BaseExternalService.");

			var types = assembly.GetTypes()
				.Where(t => t.IsClass && !t.IsAbstract && typeof(BaseExternalService).IsAssignableFrom(t))
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
		private static IServiceCollection AddMinIOStorage(this IServiceCollection services)
		{
			services.AddSingleton(sp =>
			{
				var settings = sp.GetRequiredService<IOptions<AppSettings>>().Value.Storage;

				return new MinioClient()
					.WithEndpoint(settings.Endpoint, settings.Port)
					.WithCredentials(settings.AccessKey, settings.SecretKey)
					.WithSSL(settings.UseSSL)
					.Build();
			});

			return services;
		}
	}
}
