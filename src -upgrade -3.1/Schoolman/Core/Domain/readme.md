# Domain Layer

This layer contains all entities specific to domain.

### One of the main entities of our business logic is a [Customer](./Entities/Customer.cs)

Customer has [User](./Entities/User.cs), [Role](./Entities/Role.cs), [Company](./Entities/Role.cs)

Let me explain

- A user named Jim registers in our system in order to take an educational course in **Quantum Mechanics**.
- Jim successfully completed the course, and decided to deepen his  knowledge in the field of **Molecular physics**, and also in **Thermodynamics**. Jim  successfully completed the mentioned courses.

> __So__, Jim is our __Customer__ that represent a __User__ with 'Student' __Role__


- But, already having a deep knowledge of physics, Jim decides to register as an instructor, and creates a course on __Classical mechanics__

> __So__, now Jim is our __Customer__ that represent a __User__ with  'Instructor' __Role__, at the same time remaining our __Customer__ with 'Student' __Role__ (which means he is still able to take courses and create courses)

> __As a result Jim is a User that represent 2 different Clients__   


- Jim's course in classical mechanics becomes popular, in consequence of which SpaceX sends Jim an offer to be an instructor in their company. Jim agreed of course.


>__So__ now Jim is our __Customer__ that represent a __User__ with  'Instructor'  __Role__ in Space-X __Company__, at the same time remaining our __Customer__ with 'Indivitual Instructor' __Role__, at the same time remaining our __Customer__ with student __Role__


> This way we came to the point when  one user may represent 3 different __Customer__. This is why __Customer__ is one of the main entities in this layers.