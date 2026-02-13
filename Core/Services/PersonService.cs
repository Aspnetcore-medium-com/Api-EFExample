using AutoMapper;
using Core.Domain.RepositoryContracts;
using FluentValidation;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.enums;
using Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly IMapper _mapper;
        private readonly IValidator<PersonAddRequest> _personAddRequestValidator;
        private readonly IPersonRepository _personRepository;

        public PersonService(IMapper mapper, IValidator<PersonAddRequest> validator, IPersonRepository personRepository)
        {
            _mapper = mapper;
            _personAddRequestValidator = validator;
            _personRepository = personRepository;
        }
        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest,CancellationToken cancellationToken = default)
        {
            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }
            var validationResult = _personAddRequestValidator.Validate(personAddRequest);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(Environment.NewLine, validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException($"PersonAddRequest validation failed: {Environment.NewLine}{errors}");
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
            return await _personRepository.DeletePersonAsync(person.PersonId);       

        }

        

        public async Task<IReadOnlyList<PersonResponse>> GetAllPersons(CancellationToken cancellation = default)
        {
            var persons = await _personRepository.GetAllPersonsAsync();
            List<PersonResponse> personResponses = persons.Select(p => _mapper.Map<PersonResponse>(p)).ToList();
            return personResponses;
        }

        public async Task<PersonResponse?> GetPersonById(Guid personId, CancellationToken cancellationToken = default)
        {
            var person = await _personRepository.GetPersonByIdAsync(personId,cancellationToken);
            if (person == null)
            {
                throw new KeyNotFoundException($"Person with ID {personId} not found.");
            }
            PersonResponse personResponse = _mapper.Map<PersonResponse>(person);
            return personResponse;
        }

        public async Task<IReadOnlyList<PersonResponse>> GetPersonsBy(string searchString, string columnName,CancellationToken cancellationToken = default)
        {
            var persons = await _personRepository.GetAllPersonsAsync(cancellationToken);
            List<Person> result = persons.Where(p =>
                (columnName.Equals("PersonName", StringComparison.OrdinalIgnoreCase) && p.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase)) ||
                (columnName.Equals("Email", StringComparison.OrdinalIgnoreCase) && p.Email != null && p.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)) ||
                (columnName.Equals("Address", StringComparison.OrdinalIgnoreCase) && p.Address != null && p.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase)) ||
                (columnName.Equals("DateOfBirth", StringComparison.OrdinalIgnoreCase) && p.DateOfBirth != null && p.DateOfBirth.Value.ToString("yyyy-MM-dd").Contains(searchString)) ||
                (columnName.Equals("CountryId", StringComparison.OrdinalIgnoreCase) && p.CountryId != null && p.CountryId.ToString().Contains(searchString))

            ).ToList();
            List<PersonResponse> personResponses = result.Select(p => _mapper.Map<PersonResponse>(p)).ToList();
            return personResponses;
        }

        public async Task<IReadOnlyList<PersonResponse>> GetPersonsWithSorting(string columnName, SortOptions sortOptions,CancellationToken cancellationToken = default)
        {
            var persons = await _personRepository.GetAllPersonsAsync(cancellationToken);
            if (string.IsNullOrWhiteSpace(columnName))
            {
                return persons.Select(p => _mapper.Map<PersonResponse>(p)).ToList();
            }

            var sortedPeople = columnName.ToLower() switch
            {
                "personname" => sortOptions == SortOptions.Ascending ? persons.OrderBy(p => p.PersonName) : persons.OrderByDescending(p => p.PersonName),
                "email" => sortOptions == SortOptions.Ascending ? persons.OrderBy(p => p.Email) : persons.OrderByDescending(p => p.Email),
                "address" => sortOptions == SortOptions.Ascending ? persons.OrderBy(p => p.Address) : persons.OrderByDescending(p => p.Address),
                "dateofbirth" => sortOptions == SortOptions.Ascending ? persons.OrderBy(p => p.DateOfBirth) : persons.OrderByDescending(p => p.DateOfBirth),
                "countryid" => sortOptions == SortOptions.Ascending ? persons.OrderBy(p => p.CountryId) : persons.OrderByDescending(p => p.CountryId),
                _ => throw new ArgumentException($"Invalid column name: {columnName}")
            };

            return sortedPeople.Select(p => _mapper.Map<PersonResponse>(p)).ToList();
        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest personUpdateRequest,CancellationToken cancellationToken = default)
        {
            var personToUpdate = await _personRepository.GetPersonByIdAsync(personUpdateRequest.PersonId,cancellationToken);
            if (personToUpdate == null)
            {
                throw new KeyNotFoundException($"Person with ID {nameof(personUpdateRequest.PersonId)} not found.");
            }
            //personToUpdate.PersonName = personUpdateRequest.PersonName;
            //personToUpdate.Email = personUpdateRequest.Email;
            //personToUpdate.Address = personUpdateRequest.Address;
            //personToUpdate.DateOfBirth = personUpdateRequest.DateOfBirth;
            //personToUpdate.CountryId = personUpdateRequest.CountryId;
            //personToUpdate.Gender = personUpdateRequest.Gender.ToString();
            //personToUpdate.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;
            await _personRepository.UpdatePersonAsync(personToUpdate,cancellationToken);

            PersonResponse personResponse = _mapper.Map<PersonResponse>(personToUpdate);
            return personResponse;

        }

       
    }
}
