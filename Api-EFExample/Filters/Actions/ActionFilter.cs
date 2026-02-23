using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog.Filters;
using System;

namespace Api_EFExample.Filters.Actions
{
    public class ValidationFilter<T> : IAsyncActionFilter where T : class
    {
        private readonly IValidator<T> _validator;
        
        public ValidationFilter(IValidator<T> validator) { 
            _validator = validator;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // ActionArguments.Values is ICollection<object> 
            // This safely casts the object to type T
            // Find the first action parameter that is PersonAddRequest
            //If not matching → returns null(no exception).
            var model = context.ActionArguments.Values.FirstOrDefault(x => x is T) as T;
            //Skip validation and continue.
            if (model == null) { 
                await next();
            }

            ValidationResult result = await _validator.ValidateAsync(model!);
            //short circuiting
            if (!result.IsValid) {
                context.Result = new BadRequestObjectResult(
                    result.Errors.Select(x => new { x.PropertyName,x.ErrorMessage })
                    );
                return;
            }
            await next();
        }
    }

}
