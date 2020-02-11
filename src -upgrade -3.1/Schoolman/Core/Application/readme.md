# Application Project

This layer contains all application logic. It is dependent only on the __[Domain Layer](../Domain)__, having no no dependencies on any other layer or project.

This layer defines interfaces that are implemented by outside layers.

For example

- Authentication related interfaces like IAuthService, IAuthTokenService, IAccessTokenService and etc are implemented by __[Authentication Layer](../../Infrastructure/Authentication)__
- Business and Notification related interfaces like ICustomerService, IUserService, IEmailSender and etc are implemented by __[Business Layer](../../Infrastructure/Business)__
- DB related related interfaces like IRepository, is  implemented by __[Persistence Layer](../../Infrastructure/Persistence)__

So, in the future in you need to add any new service, first of all you should add interface in __[Application Layer](./)__ layer and then implement this interface by outside layers.

> __This way we gracefully achieve Dependency Inversion Principle__
