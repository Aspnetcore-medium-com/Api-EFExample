using Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.RepositoryContracts
{
    public interface IPersonRepository
    {
        /// <summary>
        /// Get all person interface method
        /// </summary>
        /// <returns>Task<List<Person>></returns>
        public List<Person> GetAllPersons();
        /// <summary>
        /// Get a person by id 
        /// </summary>
        /// <param name="personId"></param>
        /// <returns>Task<Person></returns>
        public Person? GetPersonById(Guid personId);
        /// <summary>
        /// update a person
        /// </summary>
        /// <param name="person"></param>
        /// <returns>Task<Person></returns>
        public Task<Person> UpdatePerson(Person person);
        /// <summary>
        /// delete a person
        /// </summary>
        /// <param name="person"></param>
        /// <returns>Task<Person></returns>
        public Task<Person?> DeletePersonAsync(Guid personId);

        /// <summary>
        /// Add Person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public Task<Person?> AddPerson(Person person);
    }
}
