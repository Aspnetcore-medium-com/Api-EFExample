using Core.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services;
using Services.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Infra.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ApplicationDBContext _personDBContext;
        private readonly ILogger<PersonRepository> _logger;

        public PersonRepository(ApplicationDBContext personDBContext, ILogger<PersonRepository> logger)
        {
            _personDBContext = personDBContext;
            _logger = logger;
        }

        public async Task<Person?> AddPersonAsync(Person person, CancellationToken cancellationToken = default)
        {
            await _personDBContext.Persons.AddAsync(person, cancellationToken);
            await _personDBContext.SaveChangesAsync(cancellationToken);
            return person;
        }

        public async Task<bool> DeletePersonAsync(Guid personId, CancellationToken cancellationToken = default)
        {
            Person? person = await _personDBContext.Persons.FindAsync(new object[] { personId }, cancellationToken);

            if (person != null)
            {
                _personDBContext.Persons.Remove(person);
                await _personDBContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            return false;
        }

        public async Task<bool> ExistsByIdAsync(Guid personId, CancellationToken cancellationToken = default)
        {
            return await _personDBContext.Persons.AsNoTracking().AnyAsync(x => x.PersonId == personId);
        }

        public async Task<bool> ExistsPersonAsync(Guid personId, CancellationToken cancellationToken = default)
        {
            return await _personDBContext.Persons.AnyAsync(x => x.PersonId == personId, cancellationToken);
        }

        public async Task<bool> ExistsPersonByNameAsync(string personName, CancellationToken cancellationToken = default)
        {
            return await _personDBContext.Persons.AsNoTracking().AnyAsync(x => x.PersonName.ToLower() == personName.ToLower());
        }

        public async Task<IReadOnlyList<Person>> GetAllPersonsAsync(CancellationToken cancellationToken = default)
        {
            return await _personDBContext.Persons.Include(p => p.Country).AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<Person?> GetPersonByIdAsync(Guid personId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"{nameof(GetPersonByIdAsync)} called");
            _logger.LogDebug($"{personId} passed");

            return await _personDBContext.Persons.Include(p => p.Country).AsNoTracking().FirstOrDefaultAsync(p => p.PersonId == personId, cancellationToken);
        }

        public IQueryable<Person> GetPersonsQueryable(CancellationToken cancellationToken = default)
        {
            return _personDBContext.Persons.AsNoTracking();
        }

        public async Task<Person?> UpdatePersonAsync(Person person, CancellationToken cancellationToken = default)
        {
            Person? personInDb = await _personDBContext.Persons.FindAsync(new object[] { person.PersonId }, cancellationToken);
            if (personInDb == null) return null;
            personInDb.PersonName = person.PersonName;
            personInDb.Address = person.Address;
            personInDb.Gender = person.Gender;
            personInDb.ReceiveNewsLetters = person.ReceiveNewsLetters;

            await _personDBContext.SaveChangesAsync(cancellationToken);
            return personInDb;
        }

        public async Task<IReadOnlyList<Person>> SearchPersonsAsync(string searchTerm, string columnName, CancellationToken cancellationToken = default)
        {
            IQueryable<Person> query = _personDBContext.Persons;

            query = columnName.ToLower() switch
            {
                "personName" => query.Where(p => p.PersonName.Contains(searchTerm)),
                "email" => query.Where(p => p.Email != null && p.Email.Contains(searchTerm)),
                "address" => query.Where(p => p.Address != null && p.Address.Contains(searchTerm)),
                "dateofbirth" => query.Where(p => p.DateOfBirth != null &&
                    p.DateOfBirth.Value.ToString("yyyy-MM-dd").Contains(searchTerm)),
                "countryid" => query.Where(p => p.CountryId != null &&
                    p.CountryId.ToString().Contains(searchTerm)),
                _ => throw new ArgumentException($"Invalid column name: {columnName}")
            };

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Person>> SortPersonsAsync(string columnName, bool ascending, CancellationToken cancellation = default)
        {
            IQueryable<Person> query = _personDBContext.Persons;

            IQueryable<Person> sortedQuery = columnName.ToLower() switch
            {
                "personname" => ascending ? query.OrderBy(p => p.PersonName) : query.OrderByDescending(p => p.PersonName),
                "email" => ascending ? query.OrderBy(p => p.Email) : query.OrderByDescending(p => p.Email),
                "address" => ascending ? query.OrderBy(p => p.Address): query.OrderByDescending(p => p.Address),
                "dateofbirth" => ascending ? query.OrderBy(p => p.DateOfBirth) : query.OrderByDescending(p => p.DateOfBirth),
                "countryid" => ascending ? query.OrderBy(p => p.CountryId) : query.OrderByDescending(p => p.CountryId),
                _ => throw new ArgumentException($"Invalid column name: {columnName}", nameof(columnName))
            };
            return await sortedQuery.ToListAsync();
        }
    }
}
