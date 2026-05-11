using HotChocolate.Execution;
using Ticketing.Application.Exceptions;

namespace Ticketing.Api.GraphQL
{
    public sealed class GraphQlErrorFilter : IErrorFilter
    {

        private readonly IWebHostEnvironment _env;
        public GraphQlErrorFilter(IWebHostEnvironment env)
        {
            _env = env;
        }
        public IError OnError(IError error)
        {
            if (error.Exception is BusinessException bex)
            {
                return error
                    .WithMessage(bex.Message)
                    .WithCode(bex.Code) // => errors[].extensions.code
                    .SetExtension("httpStatus", bex.StatusCode);
            }
            // unknown error
            return error
                .WithMessage(_env.IsDevelopment() ? (error.Exception?.Message ?? error.Message) : "An error occurred while the server was processing the request.")
                .WithCode("internal_error");
        }
    }
}
