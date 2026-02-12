using ServiceContracts.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Represents the response data for a person, including identification, contact information, and personal details.
    /// </summary>
    /// <remarks>This type is typically used to transfer person-related data between application layers or in
    /// API responses.  All properties are mutable to support serialization and deserialization scenarios.</remarks>
    public class PersonResponse
    {
        public Guid PersonId { get; set; }
        public string PersonName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }

        public string? Address { get; set; }

        public Guid? CountryId { get; set; }

        public bool ReceiveNewsLetters { get; set; }
    }
}
