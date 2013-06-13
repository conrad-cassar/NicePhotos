using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Exceptions
{
    public class AccountNotVerifiedException : Exception
    {
        public AccountNotVerifiedException()
            : base("User account is not verified")
        {
        }

        public AccountNotVerifiedException(string message)
            : base(message)
        {
        }
    }
}
