using Core.Domain.RepositoryContracts;
using Services;
using Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Repositories
{
    public class PersonRepositories : IPersonRepository 
    {
        private readonly PersonDBContext _personDBContext;

        public PersonRepositories(PersonDBContext personDBContext) { 
            _personDBContext = personDBContext;
        }

        public async Task<Person?> AddPerson(Person person)
        {
            _personDBContext.Persons.Add(person);
            await _personDBContext.SaveChangesAsync();
            return person;
        }

        public async Task<Person?> DeletePersonAsync(Guid personId)
        {
           Person? person = _personDBContext.Persons.FirstOrDefault(person => person.PersonId == personId);

            if (person != null)
            {
                _personDBContext.Remove(person);
            }
            await _personDBContext.SaveChangesAsync();
            return person;
        }

        public List<Person> GetAllPersons()
        {
            return _personDBContext.Persons.ToList();
        }

        public Person? GetPersonById(Guid personId)
        {
            return _personDBContext.Persons.FirstOrDefault(person => person.PersonId == personId)  ;
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            Person? personInDb = _personDBContext.Persons.FirstOrDefault(x => x.PersonId == person.PersonId);
            personInDb.PersonName = person.PersonName;
            personInDb.Address = person.Address;
            personInDb.Gender = person.Gender;
            personInDb.ReceiveNewsLetters = person.ReceiveNewsLetters;

            await _personDBContext.SaveChangesAsync();
            return personInDb;
        }


    }
}
