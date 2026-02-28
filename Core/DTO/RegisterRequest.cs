using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    /// <summary>
    /// Represents the data required to register a new user account, including personal information and credentials.
    /// </summary>
    /// <remarks>This data transfer object is typically used to collect and validate user input during the
    /// registration process. All properties should be populated with valid values before submitting to a registration
    /// endpoint.</remarks>
    public class RegisterRequest
    {
        public string PersonName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;

        public string PhoneNumber { get; set; } = default!;

        public string ConfirmPassword { get; set; } = default!;  
    }
}
