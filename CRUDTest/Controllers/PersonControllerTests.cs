using Api_EFExample.Controllers;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTest.Controllers
{
    public class PersonControllerTests
    {
        private readonly Mock<IPersonService> _personServiceMock;

        private readonly PersonController _sut ;
        public PersonControllerTests() {
            _personServiceMock = new Mock<IPersonService>();
            _sut = new PersonController(_personServiceMock.Object);
        }

      
        [Fact]
        public async Task GetAll_WhenPersonExist_ShouldReturnOkWithResponse()
        {
            // Arrange
            var persons = new List<PersonResponse>() {
                new PersonResponse() { PersonId = Guid.NewGuid(), PersonName = "AAA" },
                new PersonResponse() {PersonId = Guid.NewGuid(), PersonName = "BBB"}
             };
            _personServiceMock.Setup(s => s.GetAllPersons(It.IsAny<CancellationToken>())).ReturnsAsync(persons);
            // Act
            var response = await _sut.GetAll(CancellationToken.None);

            // Assert
            var okResult = response.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.Should().BeEquivalentTo(persons);
        }

        [Fact]
        public async Task GetById_WhenPersonExists_ShouldReturnPerson()
        {
            // Arrange
            Guid personId = Guid.NewGuid();
            var personResponse = new PersonResponse() { PersonId = personId, PersonName = "AAA" };
            _personServiceMock.Setup(r => r.GetPersonById(personId, It.IsAny<CancellationToken>())).ReturnsAsync(personResponse);
            // Act
            var response = await _sut.GetById(personId, CancellationToken.None);
            // Assert
            response.Value.Should().NotBeNull();
            response.Value.Should().BeEquivalentTo(personResponse);

        }

        [Fact]
        public async Task GetById_WhenPersonNotExists_ShouldReturnNotFound()
        {
            // Arrange
            var personId = Guid.NewGuid();
            _personServiceMock.Setup(s => s.GetPersonById(personId, It.IsAny<CancellationToken>())).ReturnsAsync((PersonResponse?)null);
            // Act
            var response = await _sut.GetById(personId, CancellationToken.None);
            // Assert
            var notFoundResult = response.Result as NotFoundObjectResult;
            notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            var problem = notFoundResult.Value as ProblemDetails;
            problem!.Title.Should().Be("Person Not Found");
        }
    }
}
