using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.RepositoryContracts
{
    public interface ICountryRepository
    {
        /// <summary>
        /// Get all countries
        /// </summary>
        /// <returns><List<Country></returns>
        public Task<IReadOnlyList<Country>> GetAllCountriesAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// Get all countries
        /// </summary>
        /// <returns><List<Country></returns>
        public Task<Country?> GetCountryByIdAsync(Guid countryId,CancellationToken cancellationToken = default);
        /// <summary>
        /// Add country
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public Task<Country> AddCountryAsync(Country country, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete Country
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> DeleteCountryAsync(Guid countryId, CancellationToken cancellationToken = default);

        /// <summary>
        /// country by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<Country?> GetCountryByNameAsync(string countryName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Check if a country exists
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public Task<bool> ExistsAsync(Guid countryId, CancellationToken cancellationToken = default);
    }
}
