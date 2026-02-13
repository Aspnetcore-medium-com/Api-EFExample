using Entities;

namespace Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Provides data access operations for <see cref="Country"/> entities.
    /// </summary>
    public interface ICountryRepository
    {
        /// <summary>
        /// Retrieves all countries from the data store.
        /// </summary>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        /// <returns>
        /// A read-only list of all <see cref="Country"/> entities.
        /// </returns>
        Task<IReadOnlyList<Country>> GetAllCountriesAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a country by its unique identifier.
        /// </summary>
        /// <param name="countryId">
        /// The unique identifier of the country.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        /// <returns>
        /// The matching <see cref="Country"/> if found; otherwise, <c>null</c>.
        /// </returns>
        Task<Country?> GetCountryByIdAsync(
            Guid countryId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new country to the data store.
        /// </summary>
        /// <param name="country">
        /// The country entity to add.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        /// <returns>
        /// The newly added <see cref="Country"/> entity.
        /// </returns>
        Task<Country> AddCountryAsync(
            Country country,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a country by its unique identifier.
        /// </summary>
        /// <param name="countryId">
        /// The unique identifier of the country to delete.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        /// <returns>
        /// <c>true</c> if the country was successfully deleted; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> DeleteCountryAsync(
            Guid countryId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a country by its name.
        /// </summary>
        /// <param name="countryName">
        /// The name of the country.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        /// <returns>
        /// The matching <see cref="Country"/> if found; otherwise, <c>null</c>.
        /// </returns>
        Task<Country?> GetCountryByNameAsync(
            string countryName,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Determines whether a country exists in the data store.
        /// </summary>
        /// <param name="countryId">
        /// The unique identifier of the country.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        /// <returns>
        /// <c>true</c> if the country exists; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> ExistsAsync(
            Guid countryId,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsByNameAsync(string countryName,CancellationToken cancellationToken = default);
    }
}
