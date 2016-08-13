using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Crazed.KristSharp
{
    public class KristException : Exception
    {
    }

    public class KristErrorException : KristException
    {
        public readonly Dictionary<string, string> Error;

        internal KristErrorException(Dictionary<string, string> error)
        {
            Error = error;
        }

        public override string ToString()
        {
            StringBuilder error = new StringBuilder();
            error.Append(base.ToString());
            error.Append("\r\n\r\nKrist error data:\r\n");
            foreach (KeyValuePair<string, string> err in Error)
            {
                error.Append(err.Key.ToString() + ": " + err.Value.ToString() + "\r\n");
            }
            return error.ToString();
        }
    }

    public class KristPrivateKeyMissingException : KristException
    {
        public override string Message
        {
            get
            {
                return "There is no private key bound to this address.";
            }
        }
    }



    public class KristInsufficientFundsException : KristErrorException
    {
        internal KristInsufficientFundsException(Dictionary<string, string> error) : base(error)
        {
        }
    }

    public class KristAddressNotFoundException : KristErrorException
    {
        internal KristAddressNotFoundException(Dictionary<string, string> error) : base(error)
        {
        }
    }

    public class KristInvalidAddressException : KristErrorException
    { 
        internal KristInvalidAddressException(Dictionary<string, string> error) : base(error)
        {
        }
    }

    public class KristInvalidNonceException : KristErrorException
    {
        internal KristInvalidNonceException(Dictionary<string, string> error) : base(error)
        {
        }
    }

    public class KristNameTakenException : KristErrorException
    {
        internal KristNameTakenException(Dictionary<string, string> error) : base(error)
        {
        }
    }

    public class KristInvalidNameException : KristErrorException
    {
        internal KristInvalidNameException(Dictionary<string, string> error) : base(error)
        {
        }
    }

    public class KristNameNotFoundException : KristErrorException
    {
        internal KristNameNotFoundException(Dictionary<string, string> error) : base(error)
        {
        }
    }

    public class KristNotNameOwnerException : KristErrorException
    {
        internal KristNotNameOwnerException(Dictionary<string, string> error) : base(error)
        {
        }
    }
}
