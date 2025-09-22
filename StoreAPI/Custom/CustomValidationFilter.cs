using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StoreAPI.Customs
{
    public class CustomValidationFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        { 
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value!.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key,
                                  kvp => kvp.Value!.Errors
                                  .Select(e => e.ErrorMessage)
                                  .ToArray());

                var customResponse = new
                {
                    Message = "An error occurred in the validation of the data",
                    Errors = errors
                };

                context.Result = new BadRequestObjectResult(customResponse);
            }
        }
    }
}