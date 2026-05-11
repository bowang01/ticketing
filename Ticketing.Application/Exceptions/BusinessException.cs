using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Application.Exceptions
{
    public sealed class BusinessException : Exception
    {
        public string Code { get; }
        public int StatusCode { get; }
        public BusinessException(string code, string message, int statusCode = 400, Exception? inner = null)
            : base(message, inner)
        {
            Code = code;
            StatusCode = statusCode;
        }
    }
}
