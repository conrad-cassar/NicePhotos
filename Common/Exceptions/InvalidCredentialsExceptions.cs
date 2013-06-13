using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Exceptions
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException()
            : base("Invalid username or password")
        {
        }

        public InvalidCredentialsException(string message)
            : base(message)
        {
        }
    }
}
