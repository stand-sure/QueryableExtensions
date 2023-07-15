namespace Service.Filters;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Method)]
internal class ModelValidationActionFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid is false)
        {
            context.Result = new BadRequestObjectResult(context.ModelState);
        }
        
        base.OnActionExecuting(context);
    }
}