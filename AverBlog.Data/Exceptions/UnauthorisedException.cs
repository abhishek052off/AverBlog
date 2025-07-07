using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AverBlog.Data.Exceptions
{
    public class UnauthorisedException : Exception
    {
        public UnauthorisedException(string message) : base(message)
        { }
    }
}
