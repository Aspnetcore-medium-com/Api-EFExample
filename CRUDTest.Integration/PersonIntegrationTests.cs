using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Api_EFExample;
using ServiceContracts.DTO;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Entities;

namespace CRUDTest.Integration
{
    public class PersonIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;
        private readonly CustomWebApplicationFactory _factory;

        public PersonIntegrationTests(CustomWebApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();
            _factory = factory;
        }

        public async Task InitializeAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();

            // Clear all tables relevant to your tests
            dbContext.Persons.RemoveRange(dbContext.Persons);
            dbContext.Countries.RemoveRange(dbContext.Countries);
            await dbContext.SaveChangesAsync();
        }

        public Task DisposeAsync() => Task.CompletedTask;

        #region Post

        [Fact]
        public async Task PostPerson_ThenGetById_ShouldReturnCreatedPerson()
        {
            // Arrange
            var newPerson = new
            {
                PersonName = "Ursa",
                Email = "ushears1@globo.com",
                DateOfBirth = "1990-10-05",
                Gender =  1,
                CountryID = Guid.NewGuid(),
                Address = "6 Morningstar Circle",
                ReceiveNewsLetter = false
            };

            // Act 
            var postResponse = await _httpClient.PostAsJsonAsync("/api/person",newPerson);

            // Assert
            postResponse.Should().NotBeNull();
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var createdPerson = await postResponse.Content.ReadFromJsonAsync<PersonResponse>();
            createdPerson.Should().NotBeNull();
            createdPerson.PersonName.Should().Be("Ursa");

            // fetch the person
            var fetchResponse = await _httpClient.GetAsync($"/api/person/{createdPerson.PersonId}");

            // Assert
            fetchResponse.Should().NotBeNull();
            fetchResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // Act
            var fetchedPerson = await fetchResponse.Content.ReadFromJsonAsync<PersonResponse>();

            // Assert
            fetchedPerson.Should().NotBeNull();
            fetchedPerson.PersonName.Should().Be("Ursa");
        }

        [Fact]
        public async Task PostPerson_WithInvalidData_ShouldReturn400()
        {
            // Arrange
            var invalidPerson = new 
            {
                PersonName = "",
                Email = "ushears",
                DateOfBirth = "1990-10-05",
                Gender = 1,
                CountryID = Guid.NewGuid(),
                Address = "6 Morningstar Circle",
                ReceiveNewsLetter = false
            };

            // Act
            var postResponse = await _httpClient.PostAsJsonAsync("/api/person", invalidPerson);

            // Assert
            postResponse.Should().NotBeNull();
            postResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        }
        #endregion

        #region Get
        [Fact]
        public async Task GetAllPersons_ShouldReturnAllPersons()
        {
            // Arrange database
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();

            var providerName = dbContext.Database.ProviderName;
            Console.WriteLine($"Provider: {providerName}");
            dbContext.AddRange(new Person() 
            {
                PersonName = "AAA",
                Email = "one@test.com",
                DateOfBirth = DateTime.Parse("1990-10-05"),
                Gender = "Male",
                CountryId = Guid.NewGuid(),
                Address = "6 Morningstar Circle",
                ReceiveNewsLetters = false
            }, new Person() {
            
                PersonName = "BBB",
                Email = "two@test.com",
                DateOfBirth = DateTime.Parse("1990-10-05"),
                Gender = "Male",
                CountryId = Guid.NewGuid(),
                Address = "6 Morningstar Circle",
                ReceiveNewsLetters = true
            });
            await dbContext.SaveChangesAsync();

            // Act
            var getResponse = await _httpClient.GetAsync("/api/person");

            // Assert
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var persons = await getResponse.Content.ReadFromJsonAsync<List<PersonResponse>>();

            persons.Should().NotBeNull();
            persons!.Count().Should().Be(2);

        }

        #endregion

    }

}
