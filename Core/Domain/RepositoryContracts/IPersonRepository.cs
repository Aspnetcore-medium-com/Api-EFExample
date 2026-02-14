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
       Task<IReadOnlyList<Person>> GetAllPersonsAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// get persons by person id
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Person?> GetPersonByIdAsync(Guid personId,CancellationToken cancellationToken = default);
        /// <summary>
        /// update person
        /// </summary>
        /// <param name="person"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Person?> UpdatePersonAsync(Person person, CancellationToken cancellationToken = default);
        /// <summary>
        /// Delete person
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeletePersonAsync(Guid personId, CancellationToken cancellationToken = default);
        /// <summary>
        /// add person 
        /// </summary>
        /// <param name="person"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Person?> AddPersonAsync(Person person, CancellationToken cancellationToken = default);
        /// <summary>
        /// check the person exists
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> ExistsPersonByNameAsync(string personName,CancellationToken cancellationToken= default);

        /// <summary>
        /// exists by id 
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> ExistsByIdAsync(Guid personId,CancellationToken cancellationToken = default);
        /// <summary>
        /// search by search term
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="columnName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        Task<IReadOnlyList<Person>>SearchPersonsAsync(string searchTerm,string columnName, CancellationToken cancellationToken = default);
        /// <summary>
        /// sort by column name
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="ascending"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<IReadOnlyList<Person>> SortPersonsAsync(string columnName, bool ascending, CancellationToken cancellation = default);
    }
}
