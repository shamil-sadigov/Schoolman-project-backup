# Persitence Layer

This layer contains implementation of DB persitence logic using EF Core.

- It's worth to mention that actually no entity is deleted in Database in literal sense. They are just marked as deleted, but still remain in tables, providing us with ability to restore data. When user delete some data, this data actually is not deleted in DB, instead it's property 'IsDeleted' is set to TRUE, and continue remain the in table. This is called __Soft Delete__