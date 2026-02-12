using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Service contract for Country operations
    /// </summary>
    public interface ICountryService
    {
        /// <summary>
        /// Adds a new country using the specified request data.
        /// </summary>
        /// <param name="countryAddRequest">The request containing the details of the country to add. This parameter cannot be <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="CountryResponse"/>
        /// with information about the added country.</returns>
        public Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);
        
        /// <summary>
        /// Retrieves a list of all available countries.
        /// </summary>
        /// <remarks>The returned list contains country information as <see cref="CountryResponse"/>
        /// objects. The order of countries is not guaranteed.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see
        /// cref="CountryResponse"/> objects representing all available countries. If no countries are available, the
        /// list will be empty.</returns>
        public Task<List<CountryResponse>> GetAllCountries();

        /// <summary>
        /// Retrieves country information for the specified country identifier.
        /// </summary>
        /// <remarks>This method performs an asynchronous lookup for country data using the provided
        /// identifier. If no country matches the specified <paramref name="guid"/>, the result will be
        /// <c>null</c>.</remarks>
        /// <param name="guid">The unique identifier of the country to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="CountryResponse"/>
        /// with details of the country if found; otherwise, <c>null</c>.</returns>
        public Task<CountryResponse> GetCountryById(Guid guid);
    }
}
