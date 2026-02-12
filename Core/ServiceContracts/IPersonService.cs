using ServiceContracts.DTO;
using ServiceContracts.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IPersonService
    {
        /// <summary>
        /// Adds a new person to the system using the specified request data.
        /// </summary>
        /// <param name="personAddRequest">The request containing the details of the person to add. May be <see langword="null"/> to indicate no data.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="PersonResponse"/>
        /// with information about the added person.</returns>
        public Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);

        /// <summary>
        /// Get the details of a person by their unique identifier (personId). The method retrieves the person's information from the system and returns it as a <see cref="PersonResponse"/> object. If the person with the specified ID does not exist, the method may return <see langword="null"/> or throw an exception, depending on the implementation.
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public Task<PersonResponse> GetPersonById(Guid personId);
        /// <summary>
        ///  retrieves a list of all persons in the system. The method returns a collection of <see cref="PersonResponse"/> objects, each representing a person's details. If there are no persons in the system, the method may return an empty list or <see langword="null"/>, depending on the implementation.
        /// </summary>
        /// <returns></returns>
        public Task<List<PersonResponse>> GetAllPersons();

        /// <summary>
        /// searches for persons in the system based on a specified search string and column name. The method returns a list of <see cref="PersonResponse"/> objects that match the search criteria. The search string is used to filter the results based on the specified column name, which indicates the field to be searched (e.g., name, email, etc.). If no matching persons are found, the method may return an empty list or <see langword="null"/>, depending on the implementation.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public Task<List<PersonResponse>> GetPersonsBy(string searchString, string columnName);

        /// <summary>
        /// Get persons with sorting based on a specified column name and sort options. The method retrieves a list of <see cref="PersonResponse"/> objects sorted according to the provided criteria. The column name indicates the field by which the results should be sorted (e.g., name, email, etc.), while the sort options specify whether the sorting should be in ascending or descending order. If there are no persons in the system, the method may return an empty list or <see langword="null"/>, depending on the implementation.
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="sortOptions"></param>
        /// <returns></returns>
        public Task<List<PersonResponse>> GetPersonsWithSorting(string columnName, SortOptions sortOptions);

        /// <summary>
        /// Update person details based on the provided update request. 
        /// </summary>
        /// <param name="personUpdateRequest"></param>
        /// <returns></returns>
        public Task<PersonResponse> UpdatePerson(PersonUpdateRequest personUpdateRequest);
        /// <summary>
        /// deletes a person from the system based on their unique identifier (personId). The method removes the person's information from the system and returns a <see cref="PersonResponse"/> object containing details of the deleted person. If the person with the specified ID does not exist, the method may return <see langword="null"/> or throw an exception, depending on the implementation.
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public Task<PersonResponse> DeletePerson(Guid personId);
    }
}
