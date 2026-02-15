using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entities
{
    /// <summary>
    /// Represents an individual with personal and contact information.
    /// </summary>
    /// <remarks>The <see cref="Person"/> class encapsulates details such as name, email, date of birth,
    /// gender, address, and preferences for receiving newsletters. It can be used to store or transfer user profile
    /// data within applications.</remarks>
    public class Person
    {
        [Key]
        public Guid PersonId { get; set; }
        [StringLength(40)]
        public string PersonName { get; set; } = string.Empty;
        [StringLength(40)]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [StringLength(40)]
        public String? Gender { get; set; }
        public Guid? CountryId { get; set; }
        [StringLength(100)]
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        [StringLength(40)]
        public string? TIN {  get; set; }
       
        public Country? Country { get; set; }
    }
}
