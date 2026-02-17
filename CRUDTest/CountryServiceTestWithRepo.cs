using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Core.Domain.RepositoryContracts;
using Entities;
using FluentAssertions;
using Infra.Repositories;
using Moq;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTest
{
    public class CountryServiceTestWithRepo
    {
        private readonly IFixture _fixture;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICountryRepository> _countryRepositoryMock;
        private readonly CountryService _sut;

        public CountryServiceTestWithRepo() { 
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _countryRepositoryMock = _fixture.Freeze<Mock<ICountryRepository>>();
            _sut = _fixture.Create<CountryService>();

            // Fix circular reference issue
            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }


       
        [Fact]
        public async Task AddCountry_WhenNullCountryAddRequest_ShouldThrowArgumentNullException()
        {
            //Arrange
            CountryAddRequest? request = null;
            //Act
            Func<Task> act = async () => await _sut.AddCountry(request,CancellationToken.None);
            //Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddCountry_WhenCountryAlreadyExists_ShouldThrowArgumentException()
        {
            // Arrange
            var request = _fixture.Create<CountryAddRequest>();
            _countryRepositoryMock.Setup(r => r.ExistsByNameAsync(request.CountryName,It.IsAny<CancellationToken>())).ReturnsAsync(true);
            // Act
            Func<Task> act = async () => await _sut.AddCountry(request, CancellationToken.None);
            // Assert 
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddCountry_WhenValidCountryAddRequest_ShouldAddCountry()
        {
            // Arrange
            var request = _fixture.Create<CountryAddRequest>();

            var country = _fixture.Create<Country>();

            var expectedResponse = _fixture.Build<CountryResponse>().With(c => c.CountryId, country.CountryId).Create();

            _countryRepositoryMock.Setup(r => r.ExistsByNameAsync(request.CountryName,It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _mapperMock.Setup(m => m.Map<Country>(request)).Returns(country);
            _mapperMock.Setup(m => m.Map<CountryResponse>(It.IsAny<Country>())).Returns(expectedResponse);
            // Act

            var result = await _sut.AddCountry(request,CancellationToken.None);

            // Assert
            result.Should().Be(expectedResponse);

            _countryRepositoryMock.Verify(r => r.AddCountryAsync(country,It.IsAny<CancellationToken>()), Times.Once());
        }

        //public async Task<IReadOnlyList<CountryResponse>> GetAllCountries(CancellationToken cancellationToken = default)
        //{
        //    var countries = await _countryRepository.GetAllCountriesAsync(cancellationToken);
        //    return countries.Select(country => _mapper.Map<CountryResponse>(country)).ToList();
        //}
        [Fact]
        public async Task GetAllCountries_ShouldGetAllCountries() {
            // Arrange
            var countries = new List<Country>()
            {
                new Country() { CountryId = Guid.NewGuid(), CountryName = "UK" },
                new Country() { CountryId = Guid.NewGuid(), CountryName = "US"}
            };

            // Act

            // Assert
        }
    }
}
