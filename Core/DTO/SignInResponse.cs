using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class SignInResponse
    {
        public string PersonName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public DateTime Expiry {  get; set; } = DateTime.MinValue;
        
    }
}
