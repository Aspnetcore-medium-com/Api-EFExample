using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Country
    {
        [Key]
        public Guid CountryId { get; set; }
        [StringLength(100)]
        public string CountryName { get; set; } = string.Empty;
    }
}