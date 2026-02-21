using AutoMapper;
using Castle.Core.Logging;
using Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;
using Serilog;
using SerilogTimings;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.enums;
using Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly IMapper _mapper;
        private readonly IPersonRepository _personRepository;
        private readonly ILogger<PersonService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public PersonService(IMapper mapper, IPersonRepository personRepository, ILogger<PersonService> logger,IDiagnosticContext diagnosticContext)
        {
            _mapper = mapper;
            _personRepository = personRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }
        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest,CancellationToken cancellationToken = default)
        {
            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }
          
            Person person = _mapper.Map<Person>(personAddRequest);

            person.PersonId = Guid.NewGuid();
            await _personRepository.AddPersonAsync(person,cancellationToken);
            PersonResponse personResponse = _mapper.Map<PersonResponse>(person);
            return personResponse;
        }

        public async Task<bool> DeletePerson(Guid personId, CancellationToken cancellationToken = default)
        {
            Person? person = await _personRepository.GetPersonByIdAsync(personId,cancellationToken);
            if (person == null)
            {
                throw new KeyNotFoundException($"Person with ID {personId} not found.");
            }
            return await _personRepository.DeletePersonAsync(person.PersonId,cancellationToken);       

        }

        

        public async Task<IReadOnlyList<PersonResponse>> GetAllPersons(CancellationToken cancellationToken = default)
        {
            List<PersonResponse> personResponses;
            var persons = await _personRepository.GetAllPersonsAsync(cancellationToken);
            using (Operation.Time("Time for filtered persons from database"))
            {

                try
                {
                    _mapper.ConfigurationProvider.AssertConfigurationIsValid();
                    Console.WriteLine("AutoMapper configuration is VALID");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"AutoMapper configuration ERROR: {ex.Message}");
                }
                personResponses = _mapper.Map<List<PersonResponse>>(persons);
                _diagnosticContext.Set("persons", persons, destructureObjects: true);
            }
            return personResponses;
        }

        public async Task<PersonResponse?> GetPersonById(Guid personId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"{nameof(GetPersonById)} called");
            _logger.LogDebug($"{personId} is passed");
            var person = await _personRepository.GetPersonByIdAsync(personId,cancellationToken);
            if (person == null)
            {
                return null;
            }
            PersonResponse personResponse = _mapper.Map<PersonResponse>(person);
            return personResponse;
        }

        public async Task<IReadOnlyList<PersonResponse>> GetPersonsBy(string searchString, string columnName,CancellationToken cancellationToken = default)
        {
            var persons = await _personRepository.SearchPersonsAsync(searchString, columnName, cancellationToken);
            IReadOnlyList<PersonResponse> personResponses = _mapper.Map<List<PersonResponse>>(persons);
            return personResponses;
        }

        public async Task<IReadOnlyList<PersonResponse>> GetPersonsWithSorting(string columnName, SortOptions sortOptions,CancellationToken cancellationToken = default)
        {
           
            var sortedPeople = await _personRepository.SortPersonsAsync(columnName,sortOptions == SortOptions.Ascending,cancellationToken);

            IReadOnlyList<PersonResponse> sortedPersons = _mapper.Map<List<PersonResponse>>(sortedPeople);

            return sortedPersons;
        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest personUpdateRequest, CancellationToken cancellationToken = default)
        {
            if (personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(personUpdateRequest));
            }
            var personToUpdate = await _personRepository.GetPersonByIdAsync(personUpdateRequest.PersonId, cancellationToken);
            if (personToUpdate == null)
            {
                throw new KeyNotFoundException($"Person with ID {personUpdateRequest.PersonId} not found.");
            }
           
            _mapper.Map(personUpdateRequest, personToUpdate);
            await _personRepository.UpdatePersonAsync(personToUpdate, cancellationToken);

            PersonResponse personResponse = _mapper.Map<PersonResponse>(personToUpdate);
            return personResponse;

        }
       
    }
}
