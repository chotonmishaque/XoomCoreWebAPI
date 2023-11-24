using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace XoomCore.Infrastructure.Validations;

public class ValidateRequestAttribute<TRequest> : TypeFilterAttribute
{
    public ValidateRequestAttribute() : base(typeof(ValidateRequestFilterImplementation<TRequest>))
    {
    }

    private class ValidateRequestFilterImplementation<TRequest> : IAsyncActionFilter
    {
        private readonly IValidator<TRequest> _validator;

        public ValidateRequestFilterImplementation(IValidator<TRequest> validator)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.Values.FirstOrDefault() is var request)
            {
                var validationResult = await _validator.ValidateAsync((TRequest)request);

                if (!validationResult.IsValid)
                {
                    context.Result = new BadRequestObjectResult(CommonResponse<dynamic>.CreateValidationErrorList(validationResult));
                    return;
                }
            }

            await next();
        }
    }
}

