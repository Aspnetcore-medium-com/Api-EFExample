using Core.Domain.RepositoryContracts;
using Entities;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Country> AddCountryAsync(Country country, CancellationToken cancellationToken = default)
        {
            await _dbContext.Countries.AddAsync(country, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return country;
        }

        public async Task<bool> DeleteCountryAsync(Guid countryId, CancellationToken cancellationToken = default)
        {
            Country? country = await _dbContext.Countries.FindAsync(new object[] { countryId },cancellationToken);

            if (country != null)
            {
                _dbContext.Countries.Remove(country);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            return false;
        }

        public async Task<bool> ExistsAsync(Guid countryId, CancellationToken cancellationToken = default)
        {
           return await _dbContext.Countries.AnyAsync(country => country.CountryId == countryId, cancellationToken);
        }

        public async Task<IReadOnlyList<Country>> GetAllCountriesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Countries.AsNoTracking().ToListAsync(cancellationToken);
        }

        

        public async Task<Country?> GetCountryByIdAsync(Guid countryId,CancellationToken cancellationToken = default)
        {
            Country? country = await _dbContext.Countries.AsNoTracking().FirstOrDefaultAsync(country => country.CountryId == countryId,cancellationToken);
            return country;
        }

        public async Task<Country?> GetCountryByNameAsync(string countryName, CancellationToken cancellation = default)
        {
            Country? country = await _dbContext.Countries.AsNoTracking().FirstOrDefaultAsync(country => country.CountryName.ToLower() == countryName.ToLower(), cancellation);
            return country;
        }

        
    }
}
