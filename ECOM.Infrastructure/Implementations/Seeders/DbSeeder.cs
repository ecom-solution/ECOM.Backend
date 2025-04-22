using ECOM.App.Interfaces.Loggings;
using ECOM.Domain.Interfaces.DataContracts;
using ECOM.Domain.Interfaces.Seeders;
using ECOM.Shared.Library.Consts;
using ECOM.Shared.Library.Models.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text.Json;

namespace ECOM.Infrastructure.Implementations.Seeders
{
    /// <summary>
    /// Base abstract class for database seeders. Provides common functionalities
    /// such as logging, accessing app settings, unit of work, partial file hashing,
    /// and reading seed data from JSON files. Subclasses should implement the
    /// <see cref="Priority"/> and <see cref="SeedAsync"/> methods.
    /// </summary>
    public abstract class DbSeeder(
        ILog logger,
        IOptions<AppSettings> appSettings,
        [FromKeyedServices(DatabaseConstants.Main)] IUnitOfWork mainUnitOfWork) : IDbSeeder
    {
        /// <summary>
        /// Protected logger instance for logging within seeders.
        /// </summary>
        protected readonly ILog _logger = logger;

        /// <summary>
        /// Protected instance of the application settings.
        /// </summary>
        protected readonly AppSettings _appSettings = appSettings.Value;

        /// <summary>
        /// Protected instance of the main unit of work for database operations.
        /// </summary>
        protected readonly IUnitOfWork _mainUnitOfWork = mainUnitOfWork;

        /// <summary>
        /// Abstract property to define the execution priority of the seeder.
        /// Seeders with lower priority values will be executed earlier.
        /// </summary>
        public abstract int Priority { get; }

        /// <summary>
        /// Abstract method that needs to be implemented by subclasses to perform
        /// the actual database seeding logic.
        /// </summary>
        public abstract Task SeedAsync();

        /// <summary>
        /// Calculates the MD5 hash of the first specified number of bytes of a file.
        /// This can be used to quickly check if the beginning of a seed data file has changed.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="bytesToRead">The number of bytes to read from the beginning of the file for hashing.</param>
        /// <returns>A lowercase hexadecimal string representing the MD5 hash of the partial file content,
        /// or an empty string if the file does not exist.</returns>
        public virtual async Task<string> CalculatePartialFileHashAsync(string filePath, int bytesToRead)
        {
            if (!File.Exists(filePath))
            {
                return string.Empty;
            }

            try
            {
                await using var stream = File.OpenRead(filePath);
                byte[] buffer = new byte[Math.Min(bytesToRead, stream.Length)];
                await stream.ReadAsync(buffer);
                byte[] hashBytes = MD5.HashData(buffer);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
            catch (Exception ex)
            {
                _logger.Error($"Error calculating partial hash for file '{filePath}': {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Reads and deserializes a list of entities of type <typeparamref name="TEntity"/>
        /// from a JSON file located at the specified relative path.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entities to deserialize.</typeparam>
        /// <param name="relativePath">The relative path to the JSON file from the application's base directory.</param>
        /// <returns>A list of deserialized entities, or an empty list if deserialization fails or the file is empty.</returns>
        /// <exception cref="FileNotFoundException">Thrown if the JSON file is not found at the specified path.</exception>
        public virtual async Task<List<TEntity>> ReadAsync<TEntity>(string relativePath) where TEntity : class
        {
            var baseDirectory = AppContext.BaseDirectory;
            var fullPath = Path.Combine(baseDirectory, relativePath);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"Seeder file not found at path: {fullPath}");
            }

            try
            {
                await using var stream = File.OpenRead(fullPath);
                var data = await JsonSerializer.DeserializeAsync<List<TEntity>>(stream, GetJsonSerializerOptions()) ?? new List<TEntity>();
                return data;
            }
            catch (JsonException ex)
            {
                _logger.Error($"Error deserializing JSON from file '{fullPath}': {ex.Message}");
                return [];
            }
            catch (Exception ex)
            {
                _logger.Error($"Error reading file '{fullPath}': {ex.Message}");
                return [];
            }
        }

        /// <summary>
        /// Gets the default <see cref="JsonSerializerOptions"/> to be used when
        /// deserializing JSON data in the <see cref="ReadAsync{TEntity}"/> method.
        /// </summary>
        /// <returns>A <see cref="JsonSerializerOptions"/> instance configured with
        /// <see cref="JsonSerializerDefaults.Web"/> and <see cref="PropertyNameCaseInsensitive"/> set to true.</returns>
        public virtual JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = false // Typically seed data files are not indented for size efficiency
            };
        }
    }
}