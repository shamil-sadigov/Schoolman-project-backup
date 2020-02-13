# Business Layer


This layer contains implementation of business and notification logic defined in __[Application Layer](../../Core/Application)__ 

> As __[Domain Layer](../../Core/Domain)__  documentation says, Customer is on of the main entites.


Well, our layer is similar in concept.

__[CustomerManager](./Services/CustomerManager.cs)__ is one of the main services that internally use UserService, RoleService, CompanyService
and enables us to work with Customer entity.