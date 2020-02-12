# Schoolman backend architecture

## Tech stack and libraries:
- **.NET Core 3.1**
- **EF Core 3.0**
- **ASP.NET Identity**
- **Seq** *(logging service)*
- **FluentValidation** *(for validation)*
- **Automapper** *(for mapping)*
- **MediatR** *(for CQRS)*
- **xUnit** *(for testing)*
- **Mailkit** *(for sending emails)*
- **Swagger** *(for api documentation)*





## Architecture consist of the following layers:

 - __[Domain ](./Core/Domain)__ 
 - __[Application ](./Core/Application)__
 - __[Authentication ](./Infrastructure/Authentication)__ 
 - __[Business ](./Infrastructure/Business)__
 - __[Persistence ](./Infrastructure/Persistence)__
 - __[Web ](./Web)__


> Dependencies between layers are showed by arrow


### Achitecture design

![Architecture](https://res.cloudinary.com/dfmpdhjz9/image/upload/v1581490001/Schoolman%20Documentation/Architecture_design_vhmoph.png)