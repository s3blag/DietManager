using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DM.Web.Attributes
{
    public class ModelStateValidatorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;

            if (!modelState.IsValid)
                context.Result = new ContentResult()
                {
                    Content = "Data is not valid",
                    StatusCode = 400
                };
            base.OnActionExecuting(context);
        }
    }
}