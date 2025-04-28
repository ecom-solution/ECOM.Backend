using ECOM.App.Implementations.BusinessLogics.Common;
using ECOM.App.Interfaces.BusinessLogics;
using ECOM.App.Interfaces.Loggings;
using ECOM.App.Interfaces.Users;
using ECOM.Domain.Entities.Main;
using ECOM.Domain.Interfaces.DataContracts;
using ECOM.Shared.Library.Consts;
using ECOM.Shared.Library.Models.Dtos.Modules.Currency;
using ECOM.Shared.Library.Models.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ECOM.App.Implementations.BusinessLogics
{
    public class CurrencyService(
        ILog logger, IOptions<AppSettings> appSettings,
        [FromKeyedServices("Main")] IUnitOfWork mainUnitOfWork,
        ICurrentUserAccessor currentUserAccessor)
        : BaseService(logger, appSettings, mainUnitOfWork, currentUserAccessor), ICurrencyService
    {
        /// <inheritdoc/>
        public async Task<List<CurrencyDto>> GetCurrenciesAsync()
        {
            // Get the default currency code from application constants.
            var defaultCurrencyCode = ApplicationConstants.DefaultCurrency;

            // Get the repository for the Currency entity.
            var currencyRepository = _mainUnitOfWork.Repository<Currency>();

            // Build a query to retrieve all currencies, ordered by their name.
            var query = currencyRepository.OrderBy(currencyRepository.Query(), x => x.Name);

            // Execute the query and project the results into a list of CurrencyDto.
            var result = await currencyRepository.ToListAsync(query, x => new CurrencyDto
            {
                Code = x.Code,
                Name = x.Name,
                Symbol = x.Symbol,
                IsDefault = x.IsDefault
            });

            // Find the default currency based on the IsDefault property.
            var defaultCurrency = result.FirstOrDefault(x => x.IsDefault);

            // If no default currency is explicitly set, try to find a currency matching the default currency code.
            if (defaultCurrency == null)
            {
                defaultCurrency = result.FirstOrDefault(x => x.Code == defaultCurrencyCode);
                // If a currency with the default code is found, mark it as the default.
                if (defaultCurrency != null)
                    defaultCurrency.IsDefault = true;
            }

            return result;
        }
    }
}
