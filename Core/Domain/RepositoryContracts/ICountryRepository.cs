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
        public List<Country> GetAllCountries();
        /// <summary>
        /// Get all countries
        /// </summary>
        /// <returns><List<Country></returns>
        public Country? GetCountry(Guid id);
        /// <summary>
        /// Add country
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public Task<Country> AddCountry(Country country);

        /// <summary>
        /// Delete Country
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Country?> DeleteCountry(Guid id);

        /// <summary>
        /// country by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Country? GetCountryByName(string name);
    }
}
