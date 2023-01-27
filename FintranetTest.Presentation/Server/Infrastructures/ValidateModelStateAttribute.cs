using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace FintranetTest.Presentation.Server.Infrastructures;

public class ValidateModelStateAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid == false)
        {
            var result = new Result();
            foreach (var item in context.ModelState)
            {
                foreach (var error in item.Value.Errors)
                {
                    result.WithError($"{item.Key}: {error.ErrorMessage}");
                }
            }

            context.Result = new JsonResult(result.ToAPIResponse())
            {
                StatusCode = 400,
            };
        }
    }
}
