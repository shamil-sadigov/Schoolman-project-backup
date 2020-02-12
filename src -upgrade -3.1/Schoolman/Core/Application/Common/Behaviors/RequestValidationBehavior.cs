using Application.Customers;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Handler_preprocessor
{
    public sealed class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
                                                                  where TResponse : IResult
    {
        private readonly IValidator<TRequest> validator;
        private readonly ILogger<RequestValidationBehavior<TRequest, TResponse>> logger;

        public RequestValidationBehavior(IValidator<TRequest> validator,
                                         ILogger<RequestValidationBehavior<TRequest, TResponse>> logger)
        {
            this.validator = validator;
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var result = validator.Validate(request);

            // Validate request values
            if (!result.IsValid)
            {
                string[] errors = result.Errors.Select(err => err.ErrorMessage).ToArray();

                logger.LogWarning("ValidationPipelieBehavior. Validation failed for Customer registration request {@request}, {@errors}", 
                                   request, errors);
                return ReturnFailResponse(errors);
            }


            // Validate email
            // If it is customer model, we should ensure that email doesnt exist in Db
            if (request is CustomerRegistrationRequest customer)
            {
                result = await validator.ValidateAsync(request, ruleSet: "EmailDoesntExistInDb");
                if (!result.IsValid)
                {
                    string[] errors = result.Errors.Select(err => err.ErrorMessage).ToArray();

                    logger.LogWarning("ValidationPipelieBehavior. Validation failed for Customer registration request {@request}, {@errors}",
                                  request, errors);

                    return ReturnFailResponse(errors);
                }
            }

            return await next();
        }



        private TResponse ReturnFailResponse(string[] validationErrors)
        {
            // TResponse is type of IResult
            // So, TReponse should be Result or Result<> 
            // One of the Result's constructor accepts 2 params (bool isSucceeded, string[] errors)
            // and in case validation fails we create instance of that Result with errors and return as response

            // Parameters for Result constructor
            bool failed = false;
            string[] Errors = validationErrors;

            // return new Result instance with error messages
            return (TResponse)Activator.CreateInstance(typeof(TResponse), failed, Errors);
        }
    }

}
