using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Exceptions
{
    public class UserAlreadyExistException : Exception
    {
        public UserAlreadyExistException()
            : base("The entered username already exist")
        {
        }

        public UserAlreadyExistException(string message)
            : base(message)
        {
        }
    }
}
