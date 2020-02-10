using Application.Services.Business;
using FluentValidation;

namespace Application.Users.UserRegistration
{
    public class CustomerRegistrationValidator:AbstractValidator<CustomerRegistrationRequest>
    {
        private readonly ICustomerManager userManager;
        public CustomerRegistrationValidator(ICustomerManager clientManager)
        {
            this.userManager = clientManager;
        }


        public CustomerRegistrationValidator()
        {
            RuleFor(model => model.FirstName).NotEmpty()
                                             .MaximumLength(25);

            RuleFor(model => model.LastName).NotEmpty()
                                            .MaximumLength(25);

            RuleFor(model => model.Password).NotEmpty()
                                            .MinimumLength(8);

            RuleFor(model => model.Email).NotEmpty();

            RuleSet("EmailDoesntExistInDb", () =>
            {
                RuleFor(model => model.Email)
                .MustAsync(async (email, token ) =>
                {
                    bool userDoesntExist = !await userManager.ExistAsync(user => user.User.Email == email);
                    return userDoesntExist;
                });
            });

            CascadeMode = CascadeMode.Continue;
        }
    }
}
