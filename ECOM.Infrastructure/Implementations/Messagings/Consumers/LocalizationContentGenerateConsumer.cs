using ECOM.App.Interfaces.BusinessLogics;
using ECOM.App.Interfaces.Loggings;
using ECOM.Shared.Library.Models.Externals.RabbitMQ;
using ECOM.Shared.Library.Models.Settings;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ECOM.Infrastructure.Implementations.Messagings.Consumers
{
    public class LocalizationContentGenerateConsumer(
        IServiceProvider serviceProvider, 
        IOptions<AppSettings> options,
        ILog log) : IConsumer<LocalizationContentGenerateMessage>
    {
        private readonly ILog _logger = log;
        private readonly AppSettings _appSettings = options.Value;
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        public async Task Consume(ConsumeContext<LocalizationContentGenerateMessage> context)
        {
            if (context.Message.LanguageCodes != null && context.Message.LanguageCodes.Length > 0)
            {
                _logger.Information($"Received request to generate localization content for languages: {string.Join(", ", context.Message.LanguageCodes)}.");

                // Create a list to hold the tasks for each language code
                var tasks = new List<Task>();

                // Iterate through the language codes and create a task for each
                foreach (var languageCode in context.Message.LanguageCodes)
                {
                    var lc = languageCode; // Capture the languageCode in a local variable to avoid closure issues
                    tasks.Add(Task.Run(async () =>
                    {
                        _logger.Debug($"Starting task to generate localization content for language: '{lc}'.");
                        try
                        {
                            using var scope = _serviceProvider.CreateScope();
                            var languageService = scope.ServiceProvider.GetRequiredService<ILanguageService>();
                            await languageService.GenerateLocalizationContentAndUploadToStorageAsync(lc);
                            _logger.Information($"Successfully processed localization content for language: '{lc}'.");
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Error occurred while processing localization content for language: '{lc}'.", ex);
                        }
                    }));
                }

                // Wait for all the tasks to complete
                await Task.WhenAll(tasks);

                _logger.Information($"Finished processing localization content generation for all requested languages.");
            }
            else
            {
                _logger.Warning("Received localization content generation request with no language codes specified.");
            }
        }
    }
}
