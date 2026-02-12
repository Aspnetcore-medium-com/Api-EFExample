using Core.Domain.RepositoryContracts;
using Entities;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Repositories
{
    public class CountryRepositories : ICountryRepository
    {
        private readonly PersonDBContext _dbContext;

        public CountryRepositories(PersonDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Country> AddCountry(Country country)
        {
            _dbContext.Countries.Add(country);
            await _dbContext.SaveChangesAsync();
            return country;
        }

        public async Task<Country?> DeleteCountry(Guid id)
        {
            Country? country = _dbContext.Countries.FirstOrDefault(country => country.CountryId == id);
            if (country != null)
            {
                _dbContext.Remove(country);
                await _dbContext.SaveChangesAsync(true);
            }
            return country;
        }

        public List<Country>? GetAllCountries()
        {
            return _dbContext.Countries.ToList();
        }

        public Country? GetCountry(Guid id)
        {
            Country? country = _dbContext.Countries.FirstOrDefault(country => country.CountryId ==id);
            return country;
        }

        public Country? GetCountryByName(string name)
        {
            Country? country = _dbContext.Countries.FirstOrDefault(country => country.CountryName == name);
            return country;
        }
    }
}
