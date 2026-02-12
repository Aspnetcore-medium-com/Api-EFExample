using AutoMapper;
using Core.Domain.RepositoryContracts;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    /// <summary>
    /// Provides operations for managing country data, including adding new countries.
    /// </summary>
    /// <remarks>The <see cref="CountryService"/> class implements country-related business logic and data
    /// management. It maintains an in-memory collection of countries and uses an <see cref="IMapper"/> to map between
    /// data models and response objects.</remarks>
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryService(IMapper mapper,ICountryRepository countryRepository) { 
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        

        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            if (string.IsNullOrEmpty(countryAddRequest.CountryName))
            {
                throw new ArgumentException("Country name cannot be null or empty.", nameof(countryAddRequest.CountryName));
            }

            if ( _countryRepository.GetAllCountries().Any(country => country.CountryName.Equals(countryAddRequest.CountryName,StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException(message: "Country with the same name already exists.",paramName: nameof(countryAddRequest.CountryName));
            }
            Country country = _mapper.Map<Country>(countryAddRequest);

            country.CountryId = Guid.NewGuid();
            await _countryRepository.AddCountry(country);
            CountryResponse countryResponse = _mapper.Map<CountryResponse>(country);
            return await Task.FromResult(countryResponse);

        }

        public Task<List<CountryResponse>> GetAllCountries()
        {
            return Task.FromResult(_countryRepository.GetAllCountries().Select(country => _mapper.Map<CountryResponse>(country)).ToList());
        }

        public Task<CountryResponse> GetCountryById(Guid guid)
        {
           Country? country = _countryRepository.GetCountry(guid);
           if (country == null)
           {
                throw new ArgumentException(message: "Country with the specified ID does not exist.", paramName: nameof(guid));
           }
           CountryResponse countryResponse = _mapper.Map<CountryResponse>(country);
           return Task.FromResult(countryResponse);
        }
    }
}
