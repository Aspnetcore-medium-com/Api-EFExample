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
    }
}
