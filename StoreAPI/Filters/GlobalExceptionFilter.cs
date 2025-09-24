using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StoreAPI.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = new ObjectResult($"An error occurred in server: Code {StatusCodes.Status500InternalServerError}")
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}