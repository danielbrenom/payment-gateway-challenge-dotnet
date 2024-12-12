using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PaymentGateway.Api.Filter;

public class ExceptionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is null)
            return;

        context.Result = new UnprocessableEntityObjectResult(context.Exception.Message);
        context.ExceptionHandled = true;
    }
}