# Domain Layer

This layer contains all entities specific to domain.

### One of the main entities of our application is a [Customer](./Entities/Customer.cs)

Customer has [User](./Entities/User.cs), [Role](./Entities/Role.cs), [Company](./Entities/Role.cs)




## Let me explain


- A user named Steve registers in our system in order to take an educational course in **Quantum Mechanics**.


> *__So__, Steve is our __Customer__ that represent a __User__ with 'Student' __Role__*

### This is how it will be stored in relational DB 

![DB](https://gitlab.com/schoolman/schoolman-student-webapi/-/raw/documentation/create/src%20-upgrade%20-3.1/Schoolman/Core/Domain/DocumentationImages/db1.PNG)

<br />
<br />
<br />

- After taking some additional courses on physics, already having a deep knowledge Steve decides to apply to instructor Role, and creates a course on **Thermodynamics**

> __So__, now Steve is our __Customer__ that represent a __User__ with  'Instructor' __Role__, at the same time remaining our __Customer__ with 'Student' __Role__ (which means he is still able to take courses and create courses)

> __As a result Steve is a User that represent 2 different Customers__ 

### This is how it will be stored in relational DB 
*(changes in tables are highlighted in green)*

![DB](https://gitlab.com/schoolman/schoolman-student-webapi/-/raw/documentation/create/src%20-upgrade%20-3.1/Schoolman/Core/Domain/DocumentationImages/db2.PNG)


<br />
<br />

- Steve's course in **Thermodynamics** becomes popular, in consequence of which SpaceX sends Steve an offer to be an instructor in their company. Steve agreed of course.


>__So__ now Steve is our __Customer__ that represent a __User__ with  'Instructor'  __Role__ in Space-X __Company__, at the same time remaining our __Customer__ with 'Instructor' __Role__ individually, at the same time remaining our __Customer__ with student __Role__

> __As a result Steve is a User that represent 3 different Customers__ 

### This is how it will be stored in relational DB 
*(changes in tables are highlighted in green)*

![DB](https://gitlab.com/schoolman/schoolman-student-webapi/-/raw/documentation/create/src%20-upgrade%20-3.1/Schoolman/Core/Domain/DocumentationImages/db2.PNG)


> This way we came to the point when  one user may represent 3 different __Customer__. This is why __Customer__ is one of the main entities in this layers.