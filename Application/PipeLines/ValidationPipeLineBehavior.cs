using Application.Models.Wrapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PipeLines
{
    public class ValidationPipeLineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse> , IValidateMe
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationPipeLineBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if(_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(_validators.Select(vr => vr.ValidateAsync(context, cancellationToken)));
                if (!validationResults.Any(vr => vr.IsValid))
                {
                    List<string> errors = new();
                    var failures = validationResults.SelectMany(vr=>vr.Errors).Where(f=>f != null).ToList();

                    foreach (var failure in failures)
                    {
                        errors.Add(failure.ErrorMessage);
                    }
                    return (TResponse)(object)await ResponseWrapper<List<TResponse>>.FailAsync(errors);
                }
            }
            return await next();
        }
    }
}
