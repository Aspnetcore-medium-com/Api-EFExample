using AutoMapper;
using Core.Validator;
using FluentValidation;
using Moq;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.enums;
using Services;
using Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xunitCRUDTests
{
    public class PersonServiceTest
    {
        //private readonly IPersonService _personService;
        //private readonly Mock<IMapper> _mapperMock;
        //private readonly IValidator<PersonAddRequest> _validator;

        //public PersonServiceTest()
        //{
        //    _mapperMock = new Mock<IMapper>();
        //    _validator = new PersonValidator(); // Use real validator
        //                                        //_validatorMock.Setup(v => v.Validate(It.IsAny<PersonAddRequest>()))
        //                                        //   .Returns(new FluentValidation.Results.ValidationResult());
        //    _mapperMock.Setup(m => m.Map<PersonResponse>(It.IsAny<Person>()))
        //        .Returns((Person source) => new PersonResponse
        //        {
        //            PersonName = source.PersonName,
        //            CountryId = source.CountryId,
        //            DateOfBirth = source.DateOfBirth,
        //            Email = source.Email,
        //            //Gender = source.Gender?.ToString(),
        //            Address = source.Address,
        //            ReceiveNewsLetters = source.ReceiveNewsLetters,
        //            Gender = Enum.TryParse<GenderOptions>(source.Gender, out var g) ? g : null,
        //            PersonId = source.PersonId

        //        });

        //    _mapperMock.Setup(m => m.Map<Person>(It.IsAny<PersonAddRequest>()))
        //        .Returns((PersonAddRequest source) => new Person
        //        {
        //            PersonName = source.PersonName,
        //            Address = source.Address,
        //            CountryId = source.CountryId,
        //            DateOfBirth = source.DateOfBirth,
        //            Email = source.Email,
        //            Gender = source.Gender.ToString(),
        //            ReceiveNewsLetters = source.ReceiveNewsLetters

        //        });

        //    _personService = new PersonService(_mapperMock.Object, _validator);

        //}

        //#region AddPerson Tests
        //[Fact]
        //public async Task AddPerson_NullPersonAddRequest_ThrowsArgumentNullException()
        //{
        //    // Arrange
        //    PersonAddRequest? personAddRequest = null;
        //    // Act & Assert
        //    await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        //    {
        //        await _personService.AddPerson(personAddRequest);
        //    });
        //}

        //[Fact]
        ////when personname is null or empty, then AddPerson should throw ArgumentException   
        //public async Task AddPerson_EmptyPersonName_ThrowsArgumentException()
        //{
        //    // Arrange
        //    PersonAddRequest personAddRequest = new PersonAddRequest
        //    {
        //        PersonName = string.Empty
        //    };
        //    // Act & Assert
        //    await Assert.ThrowsAsync<ArgumentException>(async () =>
        //    {
        //        await _personService.AddPerson(personAddRequest);
        //    });
        //}

        //[Fact]
        ////when personname is not empty, then AddPerson should return PersonResponse object
        //public async Task AddPerson_ValidPersonAddRequest_ReturnsPersonResponse()
        //{
        //    // Arrange
        //    PersonAddRequest personAddRequest = new PersonAddRequest
        //    {
        //        PersonName = "John Doe"
        //    };
        //    // Act
        //    PersonResponse result = await _personService.AddPerson(personAddRequest);
        //    // Assert
        //    Assert.NotNull(result);
        //}

        //#endregion

        //#region Get person by id tests
        //[Fact]
        //// when personId is found, then GetPersonById should return PersonResponse object
        //public async Task GetPersonById_PersonIdFound_ReturnsPersonResponse()
        //{
        //    // Arrange
        //    PersonAddRequest personAddRequest = new PersonAddRequest
        //    {
        //        PersonName = "John Doe"
        //    };
        //    PersonResponse addedPerson = await _personService.AddPerson(personAddRequest);
        //    // Act
        //    PersonResponse result = await _personService.GetPersonById(addedPerson.PersonId);
        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(addedPerson.PersonId, result.PersonId);
        //}

        //// when personId is not found, then GetPersonById should return null
        //[Fact]
        //public async Task GetPersonById_PersonIdNotFound_ReturnsNull()
        //{
        //    // Arrange
        //    Guid nonExistentPersonId = Guid.NewGuid();

        //    // Assert
        //    await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
        //    {
        //        // Act
        //        PersonResponse result = await _personService.GetPersonById(nonExistentPersonId);
        //    });
        //}

        //#endregion
        //#region GetAllPersons Tests
        //[Fact]
        //// when there are no persons, then GetAllPersons should return empty list
        //public async Task GetAllPersons_NoPersons_ReturnsEmptyList()
        //{
        //    // Act
        //    List<PersonResponse> result = await _personService.GetAllPersons();
        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Empty(result);
        //}

        //[Fact]
        //// when there are persons, then GetAllPersons should return list of PersonResponse objects
        //public async Task GetAllPersons_PersonsExist_ReturnsListOfPersonResponse()
        //{
        //    // Arrange
        //    PersonAddRequest personAddRequest1 = new PersonAddRequest
        //    {
        //        PersonName = "John Doe"
        //    };
        //    PersonAddRequest personAddRequest2 = new PersonAddRequest
        //    {
        //        PersonName = "Jane Smith"
        //    };
        //    await _personService.AddPerson(personAddRequest1);
        //    await _personService.AddPerson(personAddRequest2);
        //    // Act
        //    List<PersonResponse> result = await _personService.GetAllPersons();
        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(2, result.Count);
        //}
        //#endregion

        //#region GetPersonsBy Tests
        //[Fact]
        //// when searchString is null or empty, then GetPersonsBy should return empty list
        //public async Task GetPersonsBy_EmptySearchString_ReturnsEmptyList()
        //{
        //    // Arrange
        //    string searchString = string.Empty;
        //    string columnName = "PersonName";
        //    // Act
        //    List<PersonResponse> result = await _personService.GetPersonsBy(searchString, columnName);
        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Empty(result);
        //}
        //[Fact]
        //// when searchString is not empty, then GetPersonsBy should return list of PersonResponse objects that match the search criteria
        //public async Task GetPersonsBy_ValidSearchString_ReturnsListOfPersonResponse()
        //{
        //    // Arrange

        //    await AddTestPersons();

        //    string searchString = "John";
        //    string columnName = "PersonName";
        //    // Act
        //    List<PersonResponse> result = await _personService.GetPersonsBy(searchString, columnName);
        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Single(result);
        //    Assert.Equal("John Doe", result[0].PersonName);
        //}

        //private async Task AddTestPersons()
        //{
        //    PersonAddRequest personAddRequest1 = new PersonAddRequest
        //    {
        //        PersonName = "John Doe",
        //        Address = "123 Main St",
        //        CountryId = Guid.NewGuid(),
        //        DateOfBirth = new DateTime(1990, 1, 1),
        //        Email = "ename@example.com",
        //        Gender = GenderOptions.Female,
        //        ReceiveNewsLetters = true

        //    };
        //    PersonAddRequest personAddRequest2 = new PersonAddRequest
        //    {
        //        PersonName = "Jane Smith",
        //        Address = "5 Main St",
        //        CountryId = Guid.NewGuid(),
        //        DateOfBirth = new DateTime(2000, 1, 1),
        //        Email = "ename1@example.com",
        //        Gender = GenderOptions.Male,
        //        ReceiveNewsLetters = false
        //    };
        //    PersonAddRequest personAddRequest3 = new PersonAddRequest
        //    {
        //        PersonName = "zak Smith",
        //        Address = "8 Main St",
        //        CountryId = Guid.NewGuid(),
        //        DateOfBirth = new DateTime(2022, 1, 1),
        //        Email = "ename1@example.com",
        //        Gender = GenderOptions.Male,
        //        ReceiveNewsLetters = false
        //    };
        //    await _personService.AddPerson(personAddRequest1);
        //    await _personService.AddPerson(personAddRequest2);
        //    await _personService.AddPerson(personAddRequest3);
        //}

        ////  when searchString is empty, then GetPersonsBy should return list of all PersonResponse objects 
        //[Fact]
        //public async Task GetPersonsBy_EmptySearchString_ReturnsAllPersons()
        //{
        //    // Arrange
        //    await AddTestPersons();
        //    string searchString = string.Empty;
        //    string columnName = "PersonName";
        //    // Act
        //    List<PersonResponse> result = await _personService.GetPersonsBy(searchString, columnName);
        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(3, result.Count);



        //}

        //#endregion

        //#region sort persons 
        ////sort persons by name in ascending order, then GetPersonsWithSorting should return list of PersonResponse objects sorted by name in ascending order
        //[Fact]
        //public async Task GetPersons_AscendingSorting_AllResult()
        //{
        //    // Arrange
        //    await AddTestPersons();
        //    SortOptions sortOption = SortOptions.Ascending;
        //    string columnName = "PersonName";
        //    // Act
        //    List<PersonResponse> result = await _personService.GetPersonsWithSorting(columnName, sortOption);
        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(3, result.Count);
        //    Assert.Equal("Jane Smith", result[0].PersonName);

        //}

        ////sort persons by name in descending order, then GetPersonsWithSorting should return list of PersonResponse objects sorted by name in descending order
        //[Fact]
        //public async Task GetPersons_DescendingSorting_AllResult()
        //{
        //    // Arrange
        //    await AddTestPersons();
        //    SortOptions sortOption = SortOptions.Descending;
        //    string columnName = "PersonName";
        //    // Act
        //    List<PersonResponse> result = await _personService.GetPersonsWithSorting(columnName, sortOption);
        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(3, result.Count);
        //    Assert.Equal("zak Smith", result[0].PersonName);
        //}

        ////update person details, then UpdatePerson should return updated PersonResponse object
        //[Fact]
        //public async Task UpdatePerson_ValidPersonUpdateRequest_ReturnsUpdatedPersonResponse()
        //{
        //    // Arrange
        //    PersonAddRequest personAddRequest = new PersonAddRequest
        //    {
        //        PersonName = "John Doe",
        //        Address = "123 ",
        //        CountryId = Guid.NewGuid(),
        //        DateOfBirth = new DateTime(1990, 1, 1),
        //        Email = "milo@example.com",
        //        Gender = GenderOptions.Male,
        //        ReceiveNewsLetters = false

        //    };
        //    PersonResponse addedPerson = await _personService.AddPerson(personAddRequest);
        //    PersonUpdateRequest personUpdateRequest = new PersonUpdateRequest
        //    {
        //        PersonId = addedPerson.PersonId,
        //        PersonName = "John Doe Updated",
        //        Address = "123 Updated",
        //        CountryId = Guid.NewGuid(),
        //        DateOfBirth = new DateTime(1990, 1, 1),
        //        Email = "milo@example.com",
        //        Gender = GenderOptions.Male,
        //        ReceiveNewsLetters = false
        //    };
        //    PersonResponse personResponse = await _personService.UpdatePerson(personUpdateRequest);
        //    Assert.NotNull(personResponse);
        //    Assert.Equal("John Doe Updated", personResponse.PersonName);

        //}

        //[Fact]
        ////update person details with invalid personId, then UpdatePerson should throw KeyNotFoundException
        //public async Task UpdatePerson_InvalidPersonId_ThrowsKeyNotFoundException()
        //{
        //    // Arrange
        //    PersonUpdateRequest personUpdateRequest = new PersonUpdateRequest
        //    {
        //        PersonId = Guid.NewGuid(),
        //        PersonName = "John Doe Updated",
        //        Address = "123 Updated",
        //        CountryId = Guid.NewGuid(),
        //        DateOfBirth = new DateTime(1990, 1, 1),
        //        Email = "milo@example.com",
        //        Gender = GenderOptions.Male,
        //        ReceiveNewsLetters = false

        //    };
        //    // Act & Assert
        //    await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
        //    {
        //        await _personService.UpdatePerson(personUpdateRequest);
        //    });
        //}
        //#endregion

        //#region delete person tests
        //[Fact]
        ////delete person with valid personId, then DeletePerson should return deleted PersonResponse object
        //public async Task DeletePerson_ValidPersonId_ReturnsDeletedPersonResponse()
        //{
        //    // Arrange
        //    PersonAddRequest personAddRequest = new PersonAddRequest
        //    {
        //        PersonName = "John Doe",
        //        Address = "123 ",
        //        CountryId = Guid.NewGuid(),
        //        DateOfBirth = new DateTime(1990, 1, 1),
        //        Email = "milo@example.com",
        //        Gender = GenderOptions.Male,
        //        ReceiveNewsLetters = false
        //    };
        //    PersonResponse addedPerson = await _personService.AddPerson(personAddRequest);
        //    // Act
        //    PersonResponse deletedPerson = await _personService.DeletePerson(addedPerson.PersonId);
        //    // Assert
        //    Assert.NotNull(deletedPerson);
        //    Assert.Equal(addedPerson.PersonId, deletedPerson.PersonId);

        //}

        //[Fact]
        ////delete person with invalid personId, then DeletePerson should throw KeyNotFoundException
        //public async Task DeletePerson_InvalidPersonId_ThrowsKeyNotFoundException()
        //{
        //    // Arrange
        //    Guid nonExistentPersonId = Guid.NewGuid();
        //    // Act & Assert
        //    await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
        //    {
        //        await _personService.DeletePerson(nonExistentPersonId);
        //    });
        //}

        //[Fact]
        ////delete person and then try to get the deleted person, then GetPersonById should throw KeyNotFoundException
        //public async Task DeletePerson_ThenGetPersonById_ThrowsKeyNotFoundException()
        //{
        //    // Arrange
        //    PersonAddRequest personAddRequest = new PersonAddRequest
        //    {
        //        PersonName = "John Doe",
        //        Address = "123 ",
        //        CountryId = Guid.NewGuid(),
        //        DateOfBirth = new DateTime(1990, 1, 1),
        //        Email = "milo@example.com",
        //        Gender = GenderOptions.Male,
        //        ReceiveNewsLetters = false


        //    };
        //    PersonResponse addedPerson = await _personService.AddPerson(personAddRequest);
        //    await _personService.DeletePerson(addedPerson.PersonId);
        //    // Act & Assert
        //    await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
        //    {
        //        await _personService.GetPersonById(addedPerson.PersonId);
        //    });
        //}


        //#endregion
    }
}