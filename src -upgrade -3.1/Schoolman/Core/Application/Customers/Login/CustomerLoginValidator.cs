using Application.Clients.Client_login;
using Application.Services.Business;
using FluentValidation;

namespace Application.Customers.Registration
{
    /// <summary>
    /// Validator of Customer registration requests
    /// </summary>
    public class CustomerLoginValidator:AbstractValidator<CustomerLoginRequest>
    {
        
        public CustomerLoginValidator()
        {
        
            RuleFor(model => model.Email).NotEmpty()
                                         .MaximumLength(50);

            RuleFor(model => model.Password).NotEmpty()
                                            .MaximumLength(50);

            RuleFor(model => model.Email).EmailAddress();


            CascadeMode = CascadeMode.Continue;
        }
    }
}
