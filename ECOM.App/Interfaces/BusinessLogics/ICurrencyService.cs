using ECOM.Shared.Library.Models.Dtos.Modules.Currency;

namespace ECOM.App.Interfaces.BusinessLogics
{
    /// <summary>
    /// Defines the contract for a service responsible for handling currency-related business logic.
    /// This interface outlines the operations that can be performed on currency data within the application.
    /// </summary>
    public interface ICurrencyService
    {
        /// <summary>
        /// Asynchronously retrieves a list of all available currencies.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.
        /// The task result contains a <see cref="List{CurrencyDto}"/> representing the currencies.</returns>
        Task<List<CurrencyDto>> GetCurrenciesAsync();
    }
}