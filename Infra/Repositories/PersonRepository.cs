using Core.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infra.Repositories
{
    public class PersonRepository : IPersonRepository 
    {
        private readonly PersonDBContext _personDBContext;

        public PersonRepository(PersonDBContext personDBContext) { 
            _personDBContext = personDBContext;
        }

        public async Task<Person?> AddPersonAsync(Person person, CancellationToken cancellationToken = default)
        {
            await _personDBContext.Persons.AddAsync(person,cancellationToken);
            await _personDBContext.SaveChangesAsync(cancellationToken);
            return person;
        }

        public async Task<bool> DeletePersonAsync(Guid personId, CancellationToken cancellationToken = default)
        {
           Person? person = await _personDBContext.Persons.FindAsync(new object[] { personId },cancellationToken);

            if (person != null)
            {
                _personDBContext.Persons.Remove(person);
                await _personDBContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            return false;
        }

        public async Task<bool> ExistsPersonAsync(Guid personId, CancellationToken cancellationToken = default)
        {
            return await _personDBContext.Persons.AnyAsync(x => x.PersonId == personId,cancellationToken);
        }

        public async Task<IReadOnlyList<Person>> GetAllPersonsAsync(CancellationToken cancellationToken = default)
        {
            return await _personDBContext.Persons.AsNoTracking().ToListAsync(cancellationToken) ;
        }

        public async Task<Person?> GetPersonByIdAsync(Guid personId, CancellationToken cancellationToken = default)
        {
            return await _personDBContext.Persons.AsNoTracking().FirstOrDefaultAsync(p => p.PersonId == personId, cancellationToken) ;
        }

        public async Task<Person?> UpdatePersonAsync(Person person,CancellationToken cancellationToken = default)
        {
            Person? personInDb =  await _personDBContext.Persons.FindAsync(new object[] { person.PersonId }, cancellationToken);
            if (personInDb == null) return null;
            personInDb.PersonName = person.PersonName;
            personInDb.Address = person.Address;
            personInDb.Gender = person.Gender;
            personInDb.ReceiveNewsLetters = person.ReceiveNewsLetters;

            await _personDBContext.SaveChangesAsync(cancellationToken);
            return personInDb;
        }

       
    }
}
