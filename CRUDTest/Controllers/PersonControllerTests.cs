using Api_EFExample.Controllers;
using AutoFixture;
using AutoFixture.AutoMoq;
using Castle.Core.Logging;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly Mock<IValidator<PersonAddRequest>> _validatorMock;
        private readonly Mock<ILogger<PersonController>> _loggerMock;
        private readonly PersonController _sut ;
        public PersonControllerTests() {
            _personServiceMock = new Mock<IPersonService>();
            _validatorMock = new Mock<IValidator<PersonAddRequest>>();
            _loggerMock = new Mock<ILogger<PersonController>>();
            _sut = new PersonController(_personServiceMock.Object,_validatorMock.Object,_loggerMock.Object);
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

        [Fact]
        public async Task Delete_WhenPersonExists_ShouldDeleteAndReturnNoContent()
        {
            // Arrange
            var personId = Guid.NewGuid();
            _personServiceMock.Setup(s => s.DeletePerson(personId, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            var response = await _sut.Delete(personId, CancellationToken.None);

            // Assert
            var NoContentResult = response as NoContentResult;
            NoContentResult!.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task Delete_WhenPersonNotFound_ShouldReturnNotFound()
        {
            // Arrange
            Guid personId = Guid.NewGuid();
            _personServiceMock.Setup(r => r.DeletePerson(personId,It.IsAny<CancellationToken>())).ReturnsAsync(false);
            // Act
            var response = await _sut.Delete(personId,CancellationToken.None);

            // Assert
            var notFoundResult = response as NotFoundObjectResult;
            notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            var problemDetails = notFoundResult.Value as ProblemDetails;
            problemDetails!.Title.Should().Be("Person not found");
        }

       

        [Fact]
        public async Task Add_WhenValidRequest_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var request = new PersonAddRequest() {  PersonName = "Test" };
            var response = new PersonResponse() { PersonName = "Test" , PersonId = Guid.NewGuid()};
            _personServiceMock.Setup(r => r.AddPerson(request, It.IsAny<CancellationToken>())).ReturnsAsync(response);
            _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult()); // empty = valid
            //Act
            var result = await _sut.Add(request, CancellationToken.None);

            // Assert
            var CreatedAtActionResult = result.Result as CreatedAtActionResult;
            CreatedAtActionResult.Should().NotBeNull();
            CreatedAtActionResult.ActionName.Should().Be("GetById");
            CreatedAtActionResult.Value.Should().BeEquivalentTo(response);
        }


    }
}
