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

        public CountryService(IMapper mapper, ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }



        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest, CancellationToken cancellationToken = default)
        {
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            if (await _countryRepository.ExistsByNameAsync(countryAddRequest.CountryName, cancellationToken)) 
            {
                throw new ArgumentException(message: "Country with the same name already exists.", paramName: nameof(countryAddRequest));
            }
            Country country = _mapper.Map<Country>(countryAddRequest);

            country.CountryId = Guid.NewGuid();
            await _countryRepository.AddCountryAsync(country, cancellationToken);
            CountryResponse countryResponse = _mapper.Map<CountryResponse>(country);
            return countryResponse;
        }

        public async Task<IReadOnlyList<CountryResponse>> GetAllCountries(CancellationToken cancellationToken = default)
        {
            var countries = await _countryRepository.GetAllCountriesAsync(cancellationToken);
            return countries.Select(country => _mapper.Map<CountryResponse>(country)).ToList();
        }

        public async Task<CountryResponse?> GetCountryById(Guid countryId, CancellationToken cancellationToken = default)
        {
            Country? country = await _countryRepository.GetCountryByIdAsync(countryId, cancellationToken);
            if (country == null)
            {
                throw new ArgumentException($"{countryId} not found exception");
            }
            CountryResponse countryResponse = _mapper.Map<CountryResponse>(country);
            return countryResponse;
        }

    }
}
