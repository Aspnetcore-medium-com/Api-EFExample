using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceContracts
{
    /// <summary>
    /// Service contract for Country operations.
    /// Handles business logic and DTO mapping.
    /// </summary>
    public interface ICountryService
    {
        /// <summary>
        /// Adds a new country using the specified request data.
        /// </summary>
        /// <param name="countryAddRequest">
        /// The request containing the details of the country to add.
        /// Cannot be null.
        /// </param>
        /// <param name="cancellationToken">
        /// Token to observe cancellation requests.
        /// </param>
        /// <returns>
        /// A <see cref="CountryResponse"/> containing details of the added country.
        /// </returns>
        Task<CountryResponse> AddCountry(
            CountryAddRequest countryAddRequest,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all available countries.
        /// </summary>
        /// <param name="cancellationToken">
        /// Token to observe cancellation requests.
        /// </param>
        /// <returns>
        /// A read-only list of <see cref="CountryResponse"/> objects.
        /// Returns an empty collection if none exist.
        /// </returns>
        Task<IReadOnlyList<CountryResponse>> GetAllCountries(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves country information by its unique identifier.
        /// </summary>
        /// <param name="countryId">
        /// The unique identifier of the country.
        /// </param>
        /// <param name="cancellationToken">
        /// Token to observe cancellation requests.
        /// </param>
        /// <returns>
        /// A <see cref="CountryResponse"/> if found; otherwise, null.
        /// </returns>
        Task<CountryResponse?> GetCountryById(
            Guid countryId,
            CancellationToken cancellationToken = default);
    }
}
