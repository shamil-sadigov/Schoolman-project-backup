# How new Test project should be created ?

-  Test project should have convention name like Test.[Tested Project Name] (examples: "Test.BusinessLayer", "Test.Persistence")
- After you created new Test project, add refference to Test.Shared library since
is has refference to all tested projects of application
- Each Test class should inherit from BasicTest class (I mean  Test.Shared.BasicTest class) since it holds required service provider
(examples: Test.BusinessLayer.UserServiceTest, Test.PersistenceLayer.RepositoryTest)