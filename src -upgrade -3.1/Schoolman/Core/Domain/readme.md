# Domain Layer

This layer contains all entities specific to domain.

### One of the main entities of our application is a [Customer](./Entities/Customer.cs)

Customer has [User](./Entities/User.cs), [Role](./Entities/Role.cs), [Company](./Entities/Company.cs)




## Let me explain


- A user named Steve registers in our system in order to take an educational course in subject **Quantum Mechanics**.


> *__So__, Steve is new __Customer__ that represent a __User__ with 'Student' __Role__*

### This is how it will be stored in relational DB 

![DB](https://downloader.disk.yandex.ru/preview/efb735cd155eb750cb2e86bc16447772e7cc726c030865b7707d01f8cb3095ab/5e42929c/LzArORicQM5S9-keIrgCgMFs6TzvDKOm6C6SD5QymDKZnwCJNek8PuaeWzHRF448dv1N6PSrrqpaekjv2X0Png==?uid=0&filename=db1.PNG&disposition=inline&hash=&limit=0&content_type=image%2Fpng&tknv=v2&owner_uid=596091530&size=2048x2048)

<br />
<br />
<br />

- After taking some additional courses on physics, already having a deep knowledge Steve decides to apply to instructor Role, and creates his own course on subject **Thermodynamics**

> __So__, now Steve is our __Customer__ that represent a __User__ with  'Instructor' __Role__, at the same time remaining our __Customer__ with 'Student' __Role__ (which means he is still able to take courses and create courses)

> __As a result Steve is a User that represent 2 different Customers__ 

### This is how it will be stored in relational DB 
*(changes in tables are highlighted in green)*

![DB](https://downloader.disk.yandex.ru/preview/9c64deee2f35774d39af000d18b73f7d6db5127a3a04e1f5697137048a4aaf2e/5e42926f/gW0g-G1wiLJs661LkzfNEMFs6TzvDKOm6C6SD5QymDIvg0KQSoIokZ5fXojz6vjrxY77FeBR0x2XpFI4Rv83fQ==?uid=0&filename=db2.PNG&disposition=inline&hash=&limit=0&content_type=image%2Fpng&tknv=v2&owner_uid=596091530&size=2048x2048)


<br />
<br />

- Steve's course in **Thermodynamics** becomes popular, in consequence of which SpaceX company sends Steve an offer to be an instructor in their company. Steve agreed of course.


>__So__ now Steve is our __Customer__ that represent a __User__ with  'Instructor'  __Role__ in Space-X __Company__, at the same time remaining our __Customer__ with 'Instructor' __Role__ individually, at the same time remaining our __Customer__ with student __Role__

> __As a result, now Steve is a User that represent 3 different Customers__ 

### This is how it will be stored in relational DB 
*(changes in tables are highlighted in green)*

![DB](https://downloader.disk.yandex.ru/preview/fbfea31599914b0ce6065905b2a965f446df7b4801f296f539fd9c348174c129/5e4292b7/MOou56N1r5ZqEQt8A5LoBsFs6TzvDKOm6C6SD5QymDL58xu9qwLQfuCv0mdeW9SdwDB2Q_SwKWTHN-bqDGJZaA==?uid=0&filename=db3.PNG&disposition=inline&hash=&limit=0&content_type=image%2Fpng&tknv=v2&owner_uid=596091530&size=2048x2048)
