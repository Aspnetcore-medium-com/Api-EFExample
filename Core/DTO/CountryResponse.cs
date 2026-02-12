using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class for Country Response
    /// </summary>
    public class CountryResponse
    {
        public Guid CountryId { get; set; }
        public string CountryName { get; set; } = string.Empty;
    }
}
