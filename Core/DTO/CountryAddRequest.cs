using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class for Add Country Request
    /// </summary>
    /// <remarks>Use this class to provide the necessary information when creating or adding a country
    public class CountryAddRequest
    {
        public string CountryName { get; set; } = string.Empty;
    }
}
