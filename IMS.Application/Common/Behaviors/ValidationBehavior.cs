
using FluentValidation;
using IMS.Application.Common.Exceptions;
using MediatR;

namespace IMS.Application.Common.Behaviors
{
    // The ValidationBehavior class is a MediatR pipeline behavior that is responsible for validating incoming requests
    // using FluentValidation validators.
    // It implements the IPipelineBehavior<TRequest, TResponse> interface, which allows it to be part of the MediatR request handling pipeline.
    public sealed class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        // The constructor takes an IEnumerable of IValidator<IRequest>,
        // which represents the collection of validators that can be applied to the incoming request.
        // Validators: they are responsible for defining the validation rules for specific request types. Each validator implements the IValidator<T> interface from FluentValidation,
        // where T is the type of request it validates. The ValidationBehavior will use these validators to validate the incoming request before it reaches the actual request handler.

        // ex : if you have a CreateProductCommand that represents a request to create a new product,
        // you might have a CreateProductCommandValidator that defines the validation rules for that command
        // (e.g., ensuring that the product name is not empty and the price is greater than zero).
        // The ValidationBehavior will automatically apply this validator to any incoming CreateProductCommand requests.
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // 1) If there are no validators for the incoming request,
            // it simply calls the next delegate in the pipeline and returns its result.
            if (!_validators.Any())
                return await next();

            // 2) If there are validators, it creates a ValidationContext<TRequest> using the incoming request.
            var context = new ValidationContext<TRequest>(request);

            // 3) It then executes all the validators in parallel using Task.WhenAll and collects their results.

            // ex : if you have multiple validators for a single request type
            // (e.g., CreateProductCommandValidator and another validator that checks for specific business rules),
            // both validators will be executed concurrently, and their results will be collected together.
            var results = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            // 4) Finally, it aggregates any validation failures from the results and can handle them as needed
            // (e.g., by throwing an exception or returning a specific response).
            var failures = results
                .SelectMany(r => r.Errors)
                .Where(f => f is not null)
                .ToList();

            // 5) If there are no validation failures,
            // it proceeds to the next delegate in the pipeline,
            // allowing the request to be processed by the actual request handler.
            if (failures.Count == 0)
                return await next();

            // 6) If there are validation failures, it aggregates them into a dictionary
            // where the key is the property name and the value is an array of error messages. 
            var errors = failures
                .GroupBy(f => f.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ErrorMessage).Distinct().ToArray()
                );

            // 7) Finally, it throws a ValidationException with the aggregated errors,
            // which can be caught and handled by the application's exception handling middleware or logic.
            throw new IMS.Application.Common.Exceptions.ValidationException(errors);

            // Note that the ValidationException is a custom exception defined in the IMS.Application.Common.Exceptions namespace,
            // and it is designed to encapsulate the validation errors in a structured way,
            // allowing for better error handling and reporting in the application.
        }
    }
}
