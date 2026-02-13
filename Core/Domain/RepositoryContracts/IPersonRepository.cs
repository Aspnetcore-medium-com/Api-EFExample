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
       /// Get all persons
       /// </summary>
       /// <param name="cancellationToken"></param>
       /// <returns></returns>
        public Task<IReadOnlyList<Person>> GetAllPersonsAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// get persons by person id
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Person?> GetPersonByIdAsync(Guid personId,CancellationToken cancellationToken = default);
        /// <summary>
        /// update person
        /// </summary>
        /// <param name="person"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Person?> UpdatePersonAsync(Person person, CancellationToken cancellationToken = default);
        /// <summary>
        /// Delete person
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> DeletePersonAsync(Guid personId, CancellationToken cancellationToken = default);
        /// <summary>
        /// add person 
        /// </summary>
        /// <param name="person"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Person?> AddPersonAsync(Person person, CancellationToken cancellationToken = default);
        /// <summary>
        /// check the person exists
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> ExistsPersonByNameAsync(string personName,CancellationToken cancellationToken= default);

        /// <summary>
        /// exists by id 
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> ExistsByIdAsync(Guid personId,CancellationToken cancellationToken = default);
    }
}
