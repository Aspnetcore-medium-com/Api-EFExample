using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    public class PersonNotFoundException : ArgumentException
    {
        public PersonNotFoundException() { }

        public PersonNotFoundException(string message) : base(message) { }

        public PersonNotFoundException(string message, Exception innerException) : base(message, innerException) { };
    }
}
