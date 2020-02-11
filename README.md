# Schoolman backend architecture

## Tech stack and libraries:
> **.NET Core 3.1**

> **EF Core 3.0**

> **ASP.NET Identity**

> **Seq** *(logging service)*

> **FluentValidation** *(for validation)*

> **Automapper** *(for mapping)*

> **MediatR** *(for CQRS)*

> **xUnit** *(for testing)*

> **Mailkit** *(for sending emails)*





## Architecture consist of the following layers:

 - __[Domain ](./Core/Domain)__ 
 - __[Application ](./Core/Application)__
 - __[Authentication ](./Infrastructure/Authentication)__ 
 - __[Business ](./Infrastructure/Business)__
 - __[Persistence ](./Infrastructure/Persistence)__
 - __[Web ](./Web)__


> Dependencies between layers are showed by arrow


### Achitecture design

![Architecture](https://downloader.disk.yandex.ru/preview/feb6461f17df0198155a7204d55594a7bd417aa2f877bd85a856d3afaa931433/5e42d05d/BkUcR9lE3g4VxW7W6HMS7F5AzR6ilUI8f8ONjO8ajXPxtetPKQQv6ND3_UUjLeLgfV1WHNYXlzK4jjXIqhGWCQ==?uid=0&filename=Architecture+design.png&disposition=inline&hash=&limit=0&content_type=image%2Fpng&tknv=v2&owner_uid=596091530&size=2048x2048)