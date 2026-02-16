using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Core.Domain.RepositoryContracts;
using FluentAssertions;
using FluentValidation;
using Infra.Repositories;
using Moq;
using ServiceContracts.DTO;
using Services;
using Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//        public PersonRepository(ApplicationDBContext personDBContext)
//         public PersonService(IMapper mapper, IPersonRepository personRepository)
namespace CRUDTest
{
    public class PersonServiceWithRepoTest
    {
        private readonly Mock<IMapper> _mapperMock;
        //private readonly IValidator<PersonAddRequest> _validator;
        private readonly IFixture _fixture;
        private readonly Mock<IPersonRepository> _personRepositoryMock;
        private readonly PersonService _sut; //system under test
        public PersonServiceWithRepoTest()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            // Fix circular reference issue
            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            //end
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _personRepositoryMock = _fixture.Freeze<Mock<IPersonRepository>>();
            _sut = _fixture.Create<PersonService>();
        }


        //public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest,CancellationToken cancellationToken = default)

        [Fact]
        public async Task AddPerson_WhenRequestIsNull_ShouldThrowArgumentException()
        {
            //Arrange 
            PersonAddRequest personAddRequest = null;
            //Act
            Func<Task> act = async () => await _sut.AddPerson(personAddRequest);
            //Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        //public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest, CancellationToken cancellationToken = default)
        //{
        //    if (personAddRequest == null)
        //    {
        //        throw new ArgumentNullException(nameof(personAddRequest));
        //    }

        //    Person person = _mapper.Map<Person>(personAddRequest);

        //    person.PersonId = Guid.NewGuid();
        //    await _personRepository.AddPersonAsync(person, cancellationToken);
        //    PersonResponse personResponse = _mapper.Map<PersonResponse>(person);
        //    return personResponse;
        //}

        [Fact]
        public async Task AddPerson_WhenValidRequest_ShouldAddPersonAndReturnPersonResponse()
        {
            // Arrange
            var request = _fixture.Create<PersonAddRequest>();

            var PersonEntity = _fixture.Build<Person>()
                                         .With(p => p.PersonId, Guid.NewGuid())
                                         .Create();
            var expectedResponse = _fixture.Create<PersonResponse>();

            _mapperMock.Setup(m => m.Map<Person>(request)).Returns(PersonEntity);
            _mapperMock.Setup(m => m.Map<PersonResponse>(It.IsAny<Person>())).Returns(expectedResponse);
            _personRepositoryMock.Setup(p => p.AddPersonAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Person p, CancellationToken _) => p);


            // Act
            var result = await _sut.AddPerson(request,CancellationToken.None);

            // Assert

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResponse);

            _personRepositoryMock.Verify(r => r.AddPersonAsync(It.Is<Person>(p => p.PersonId != Guid.Empty),It.IsAny<CancellationToken>()),Times.Once);
        }
    }
}
