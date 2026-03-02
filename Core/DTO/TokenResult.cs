using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class TokenResult
    {
        public string Token { get; set; } = string.Empty;

        public DateTime ExpiryTime { get; set; } = DateTime.MinValue;

        public string RefreshToken { get; set; } = default!;
        public DateTime RefreshTokenExpiry {  get; set; } = DateTime.MinValue; 
    }
}
