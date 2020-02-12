using Application.Services.Business;
using FluentValidation;

namespace Application.Customers.Registration
{
    /// <summary>
    /// Validator of Customer registration requests
    /// </summary>
    public class CustomerRegistrationValidator:AbstractValidator<CustomerRegistrationRequest>
    {
        private readonly ICustomerManager customerManager;


        public CustomerRegistrationValidator(ICustomerManager customerManager)
        {
            this.customerManager = customerManager;
        
            RuleFor(model => model.FirstName).NotEmpty()
                                             .MaximumLength(25);

            RuleFor(model => model.LastName).NotEmpty()
                                            .MaximumLength(25);

            RuleFor(model => model.Password).NotEmpty()
                                            .MinimumLength(8);

            RuleFor(model => model.Email).EmailAddress();


            // Validation rule that is used only if above rules are passed
            // In this rule we want to ensure that no customer is registered with the same email
            RuleSet("EmailDoesntExistInDb", () =>
            {
                RuleFor(model => model.Email)
                .MustAsync(async (email, token ) =>
                {
                    bool userDoesntExist = !await customerManager.ExistAsync(customer => customer.User.Email == email);
                    return userDoesntExist;
                });
            });

            CascadeMode = CascadeMode.Continue;
        }
    }
}
