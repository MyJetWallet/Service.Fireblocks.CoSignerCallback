using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Fireblocks.CoSignerCallback.Domain.Exceptions
{
    public class ApiKeysAreNotActivatedException : Exception
    {
        public ApiKeysAreNotActivatedException(string message) : base(message)
        {
        }
    }
}
