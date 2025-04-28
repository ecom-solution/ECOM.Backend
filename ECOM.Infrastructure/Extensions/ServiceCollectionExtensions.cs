using ECOM.Domain.Interfaces.Emails;
using ECOM.Domain.Interfaces.Hashs;
using ECOM.Domain.Interfaces.Mappings;
using ECOM.Domain.Interfaces.Messagings;
using ECOM.Domain.Interfaces.Notifications;
using ECOM.Domain.Interfaces.OTPs;
using ECOM.Domain.Interfaces.DataContracts;
using ECOM.Domain.Interfaces.Seeders;
using ECOM.Infrastructure.Database.Main;
using ECOM.Infrastructure.Database.MainLogging;
using ECOM.Infrastructure.Implementations.DataContracts.UnitOfWorks;
using ECOM.Infrastructure.Implementations.Emails;
using ECOM.Infrastructure.Implementations.Hashs;
using ECOM.Infrastructure.Implementations.Mappings;
using ECOM.Infrastructure.Implementations.Messagings;
using ECOM.Infrastructure.Implementations.Messagings.Consumers;
using ECOM.Infrastructure.Implementations.OTPs;
using ECOM.Infrastructure.Implementations.Seeders;
using ECOM.Shared.Library.Consts;
using ECOM.Shared.Library.Models.Settings;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using System.Reflection;
using ECOM.Infrastructure.Implementations.Notifications.SignalR.Providers;
using ECOM.Infrastructure.Implementations.Notifications.SignalR.Hubs;
using ECOM.Infrastructure.Implementations.Notifications.SignalR;
using ECOM.Domain.Interfaces.Storages;
using ECOM.Infrastructure.Implementations.Storages;

namespace ECOM.Infrastructure.Extensions
{
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Registers infrastructure services including DbContexts, UnitOfWork, Email, Notification, RabbitMQ, SignalR, and Storage.
		/// </summary>
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			// Get AppSettings (already configured in Program.cs)
			var appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>() ?? throw new Exception("Can not initialize AppSettings");
			var rabbitSettings = appSettings.RabbitMQ;
			var smtpSettings = appSettings.Smtp;
			var storageSettings = appSettings.Storage;

			// Register DbContexts
			services.AddDbContextPool<MainDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString(nameof(MainDbContext))));

			services.AddDbContextPool<MainLoggingDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString(nameof(MainLoggingDbContext))));

			// Register UnitOfWork with keyed DI
			services.AddKeyedScoped<IUnitOfWork, MainUnitOfWork>(DatabaseConstants.Main);
			services.AddKeyedScoped<IUnitOfWork, MainLoggingUnitOfWork>(DatabaseConstants.MainLogging);

			// Register all seeders implementing IDbSeeder via reflection
			var seederType = typeof(IDbSeeder);
			var seederTypes = Assembly.GetExecutingAssembly()
				.GetTypes()
				.Where(t => !t.IsAbstract && !t.IsInterface && seederType.IsAssignableFrom(t));

			foreach (var type in seederTypes)
			{
				services.AddTransient(seederType, type);
			}
			services.AddScoped<IDbSeederModule, DbSeederModule>();

			// Register AutoMapper and mapper interface
			services.AddAutoMapper(typeof(ApplicationMapper).Assembly);
			services.AddSingleton<IMap, ApplicationMapper>();

			// Register MinIO client
			services.AddSingleton(sp =>
			{
				return new MinioClient()
					.WithEndpoint(storageSettings.Endpoint, storageSettings.Port)
					.WithCredentials(storageSettings.AccessKey, storageSettings.SecretKey)
					.WithSSL(storageSettings.UseSSL)
					.Build();
			});

            // Register MinIOStorage
            services.AddScoped<IStorage, MinIOStorage>();

			// Register email and notification services
			services.AddScoped<IEmailSender, ZohoEmailSender>();
			services.AddScoped<INotificationSender, SignalRNotificationSender<NotificationHub>>();

			// Configure SignalR and user ID provider
			services.AddSignalR();
			services.AddSingleton<IUserIdProvider, UserIdProvider>();

			// Configure MassTransit with RabbitMQ and consumers
			services.AddScoped<IPublisher, RabbitMQPublisher>();
			services.AddMassTransit(x =>
			{
				// Register consumers
				x.AddConsumer<EmailConsumer>();
				x.AddConsumer<NotificationConsumer>();
				x.AddConsumer<LocalizationContentGenerateConsumer>();

				x.UsingRabbitMq((context, cfg) =>
				{
					var rabbitSettings = appSettings.RabbitMQ;

					cfg.Host(rabbitSettings.Host, rabbitSettings.VirtualHost, h =>
					{
						h.Username(rabbitSettings.Username);
						h.Password(rabbitSettings.Password);
					});

					// 👇 Configure queues
					cfg.ReceiveEndpoint(rabbitSettings.EmailQueue, e =>
					{
						e.ConfigureConsumer<EmailConsumer>(context);
						e.BindDeadLetterQueue(rabbitSettings.EmailQueueDLQ);
					});

					cfg.ReceiveEndpoint(rabbitSettings.NotifyQueue, e =>
					{
						e.ConfigureConsumer<NotificationConsumer>(context);
						e.BindDeadLetterQueue(rabbitSettings.NotifyQueueDLQ);
					});

                    cfg.ReceiveEndpoint(rabbitSettings.LocalizationContentGenerateQueue, e =>
                    {
                        e.ConfigureConsumer<LocalizationContentGenerateConsumer>(context);
                        e.BindDeadLetterQueue(rabbitSettings.LocalizationContentGenerateQueueDLQ);
                    });
                });
			});

			// Register PasswordHasher
			services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();

			// Register TotpService
			services.AddSingleton<ITotpService, TotpService>();

			return services;
		}
	}
}
