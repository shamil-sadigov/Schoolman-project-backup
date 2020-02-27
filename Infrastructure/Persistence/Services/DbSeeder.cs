using Application.Customers;
using Application.Services.Business;
using Domain.Entities;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Services
{
    public class DbSeeder
    {
        private readonly SchoolmanContext context;
        private readonly ICustomerManager customerManager;

        public DbSeeder(SchoolmanContext schoolmanContext,
                        ICustomerManager customerManager)
        {
            context = schoolmanContext;
            this.customerManager = customerManager;
        }

        public async Task SeedAsync()
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (!await context.Courses.AnyAsync())
                    {
                        #region Create course on quantum mechanics

                        Customer customer_werner = await CreateCustomerAsync("Werner", "Heisenberg");
                        Instructor instructor_werner = await CreateInstructorAsync(customer_werner.Id);
                        await CreateCourseAsync(instructor_werner.Id,
                                                name: "Quantum Mechanics for Everyone",
                                                description: "Learn the fundamental notions of quantum mechanics at a level that is accessible to everyone.",
                                                about: "Quantum Mechanics for Everyone is a four-week long MOOC that teaches the basic ideas of quantum mechanics " +
                                                        "with a method that requires no complicated math beyond taking square roots (and you can use a calculator " +
                                                        "for that). Quantum theory is taught without “dumbing down” any of the material, giving you the same version " +
                                                        "experts use in current research. We will cover the quantum mystery of the two-slit experiment and advanced " +
                                                        "topics that include how to see something without shining light on it (quantum seeing in the dark) and bunching" +
                                                        "effects of photons (Hong-Ou-Mandel effect).",
                                                imageUri: "https://res.cloudinary.com/dfmpdhjz9/image/upload/v1582808928/Schoolman%20course%20seeding%20images/VR/quantum_physics_ti7iv8.gif",
                                                duration: TimeSpan.FromHours(60),
                                                skillsDescription: new string[]
                                                {
                                                    "Understand what a quantum particle is in the world of the ultrasmall",
                                                    "Learn the basics of probability theory",
                                                    "Discover what spin is and how it is manipulated by magnets",
                                                    "Explain what the quantum mystery is",
                                                    "Apply quantum ideas to understand partial reflection of light, interaction-free measurements, and particle indistinguishability"
                                                },
                                                courseType: CourseType.VR);

                        #endregion

                        #region Create course on neurobiology

                        var customer_anastasia = await CreateCustomerAsync("Anastasia", "Kazantseva");

                        Instructor instructor_anastasia = await CreateInstructorAsync(customer_anastasia.Id);

                        await CreateCourseAsync(instructor_anastasia.Id,
                                               name: "From Brain to Symptom – Introduction to Neuroscientific Psychiatry",
                                               description: "The first MOOC to teach modern neuroscientific psychiatry. Learn about functional neuroanatomy " +
                                               "in relation to psychiatric conditions and their treatment..",

                                               about: "Neuroscientific psychiatry is the new generation’s brain-based understanding of human behavior in " +
                                               "health and disease. Emerging over the past 10-15 years using new technologies such as brain imaging and" +
                                               " neurogenetics, this combination of classic medicine and cognitive neuroscience has rapidly changed our views" +
                                               " of psychiatric conditions and their treatment. Basic concepts and principles from this field can be used in " +
                                               "clinical situations for diagnostics and treatment.",
                                               imageUri: "https://res.cloudinary.com/dfmpdhjz9/image/upload/v1582808911/Schoolman%20course%20seeding%20images/VR/neuroBiology_jf6cuz.gif",
                                               duration: TimeSpan.FromHours(85),
                                               skillsDescription: new string[]
                                               {
                                                    "An overview of the structures of the human brain and their corresponding functions",
                                                    "Reasoning about general disease models in psychiatry",
                                                    "Applied understanding of cognition and emotion as distinct flows of information in the brain",
                                                    "An overview of contemporary personality models",
                                                    "How to reflect on how disturbances of brain functioning can manifest in clinical conditions"
                                               },
                                               courseType: CourseType.VR);

                        #endregion

                        #region Create course on hr

                        var customer_harley = await CreateCustomerAsync("Harley", "Daisy");

                        Instructor instructor_harley = await CreateInstructorAsync(customer_harley.Id);

                        await CreateCourseAsync(instructor_harley.Id,
                                               name: "Managing Human Resources in the Hospitality and Tourism Industry",
                                               description: "Understand essential human resources concepts and theories and " +
                                               "analyze contemporary issues in the management of human capital",

                                               about: "In this business and management course, we will analyze contemporary " +
                                               "issues in the management of human capital in the hotel and tourism industry, within" +
                                               " both macro- and micro-perspectives. You will learn how organizational culture impacts " +
                                               "human capital, how to effectively staff your team, leadership skills and how to manage" +
                                               " employee motivation.We will also discuss how different cultures approach human resource" +
                                               " management(HRM). Note that this course is priced at USD $198.",

                                               imageUri: "https://res.cloudinary.com/dfmpdhjz9/image/upload/v1582808886/Schoolman%20course%20seeding%20images/InClass/HR_fryo5g.gif",
                                               duration: TimeSpan.FromHours(40),
                                               skillsDescription: new string[]
                                               {
                                                    "Evaluate and discuss the knowledge and theories of managing human resources applicable to the hotel and tourism industry",
                                                    "Appraise and analyze human resources functions and employee motivation in the context of the hotel and tourism environment",
                                                    "Assess the relationship between the hotel and tourism industry and their society; and evaluate the impacts of social, economical, and cultural factors on managing human resources",
                                                    "Evaluate the impacts and factors that affect the development of organizational culture",
                                                    "Analyze and evaluate HR research journals to identify applicable methodologies",
                                                    "Analyze and appraise related numerical and graphical data regarding managing human resources, and develop solutions for industry practitioners"
                                               },
                                               courseType: CourseType.InClass);

                        #endregion

                        #region Create course on analyrical Chemistry

                        var customer_lavoisier = await CreateCustomerAsync("Antoine", "Lavoisier");
                        Instructor instructor_lavoisier = await CreateInstructorAsync(customer_lavoisier.Id);

                        await CreateCourseAsync(instructor_lavoisier.Id,
                                               name: "Basic Analytical Chemistry",
                                               description: "Gain a physical understanding of the principles of analytical chemistry and their application in scientific research",

                                               about: "Analytical chemistry takes a prominent position among all fields of experimental sciences, ranging from fundamental studies" +
                                               " of Nature to industrial or clinical applications. Analytical chemistry covers the fundamentals of experimental and analytical methods " +
                                               "and the role of chemistry around us.",

                                               imageUri: "https://res.cloudinary.com/dfmpdhjz9/image/upload/v1582808898/Schoolman%20course%20seeding%20images/VR/Chemistry_my7ql6.gif",
                                               duration: TimeSpan.FromHours(70),
                                               skillsDescription: new string[]
                                               {
                                                    "A basic background in the chemical principles, particularly important to analytical chemistry",
                                                    "The ability to judge the accuracy and precision of experimental data and to show how these judgments can be sharpened by the application of statistical methods",
                                                    "A wide range of techniques that are useful in modern analytical chemistry",
                                                    "Evaluate the impacts and factors that affect the development of organizational culture",
                                                    "The skills needed to solve analytical problems in a quantitative manner",
                                                    "Laboratory skills to obtain high-quality analytical data"
                                               },
                                               courseType: CourseType.VR);

                        #endregion

                        #region Create course on computer science

                        var customer_martin = await CreateCustomerAsync("Robert", "Martin");

                        Instructor instructor_martin = await CreateInstructorAsync(customer_martin.Id);

                        await CreateCourseAsync(instructor_martin.Id,
                                               name: "CS50's Computer Science for Business Professionals",
                                               description: "This is CS50’s introduction to computer science for business professionals",

                                               about: "This is CS50’s introduction to computer science for business professionals, " +
                                               "designed for managers, product managers, founders, and decision-makers more generally. " +
                                               "Whereas CS50 itself takes a bottom-up approach, emphasizing mastery of low-level concepts " +
                                               "and implementation details thereof, this course takes a top-down approach, emphasizing" +
                                               " mastery of high-level concepts and design decisions related thereto. Through lectures on " +
                                               "computational thinking, programming languages, internet technologies, web development," +
                                               " technology stacks, and cloud computing, this course empowers you to make technological " +
                                               "decisions even if not a technologist yourself. You’ll emerge from this course with first-hand " +
                                               "appreciation of how it all works and all the more confident in the factors that should guide " +
                                               "your decision-making.",

                                               imageUri: "https://res.cloudinary.com/dfmpdhjz9/image/upload/v1582808887/Schoolman%20course%20seeding%20images/InDemand/comp_science_nszmcp.gif",
                                               duration: TimeSpan.FromHours(120),
                                               skillsDescription: new string[]
                                               {
                                                    "Computational thinking",
                                                    "Programming languages",
                                                    "Internet technologies",
                                                    "Web development",
                                                    "Technology stacks",
                                               },
                                               courseType: CourseType.Online);
                        #endregion

                        #region Create course on database

                        var customer_raymond = await CreateCustomerAsync("Raymond", "Boyce");

                        Instructor instructor_raymond = await CreateInstructorAsync(customer_raymond.Id);

                        await CreateCourseAsync(instructor_raymond.Id,
                                               name: "SQL for Data Science",
                                               description: "Learn how to use and apply the powerful language of SQL to better communicate and extract " +
                                               "data from databases - a must for anyone working in the data science field.",

                                               about: "Much of the world's data lives in databases. SQL (or Structured Query Language)" +
                                               " is a powerful programming language that is used for communicating with and extracting various " +
                                               "data types from databases. A working knowledge of databases and SQL is necessary to advance " +
                                               "as a data scientist or a machine learning specialist. The purpose of this course is to introduce " +
                                               "relational database concepts and help you learn and apply foundational knowledge of the SQL" +
                                               " language. It is also intended to get you started with performing SQL access in a data science environment.",

                                               imageUri: "https://res.cloudinary.com/dfmpdhjz9/image/upload/v1582808926/Schoolman%20course%20seeding%20images/InDemand/Databases2_nsnlaq.gif",
                                               duration: TimeSpan.FromHours(95),
                                               skillsDescription: new string[]
                                               {
                                                    "Learn and apply foundational knowledge of the SQL language",
                                                    "How to create a database in the cloud",
                                                    "How to use string patterns and ranges to query data",
                                                    "How to sort and group data in result sets and by data type",
                                               },
                                               courseType: CourseType.Online);
                        #endregion

                        #region Create course on sorting algorithms

                        var customer_neumann = await CreateCustomerAsync("John von", "Neumann");

                        Instructor instructor_neumann = await CreateInstructorAsync(customer_neumann.Id);

                        await CreateCourseAsync(instructor_neumann.Id,
                                               name: "Sorting Algorithm Design and Analysis",
                                               description: "Learn about the core principles of computer science: algorithmic thinking and computational problem solving",

                                               about: "How do you optimally encode a text file? How do you find shortest paths in a map? How do you" +
                                               " design a communication network? How do you route data in a network? What are the limits of efficient " +
                                               "computation? This course, part of the Computer Science Essentials for Software Development Professional" +
                                               " Certificate program, is an introduction to design and analysis of algorithms, and answers along the way" +
                                               " these and many other interesting computational questions",

                                               imageUri: "https://res.cloudinary.com/dfmpdhjz9/image/upload/v1582808920/Schoolman%20course%20seeding%20images/InDemand/Sorting_Algorithms_ebnhko.gif",
                                               duration: TimeSpan.FromHours(55),
                                               skillsDescription: new string[]
                                               {
                                                    "How to represent data in ways that allow you to access it efficiently in the ways you need to",
                                                    "How to analyze the efficiency of algorithms",
                                                    "How to bootstrap solutions on small inputs into algorithmic solutions on bigger inputs",
                                               },
                                               courseType: CourseType.Online);

                        #endregion

                        #region Create course on art&design

                        var customer_salvador = await CreateCustomerAsync("Salvador", "Dalí ");

                        Instructor instructor_salvador = await CreateInstructorAsync(customer_salvador.Id);

                        await CreateCourseAsync(instructor_salvador.Id,
                                               name: "Art and Design in the Digital Age",
                                               description: "Explore and discover how art changes technology and how technology changes art",

                                               about: "We tend to think of art and technology as two separate, almost opposite things. " +
                                               "But what if we showed you that the development of technology owes its debt to artists? " +
                                               "And that art would not be what it is, without technology? 'The digital age, born out of'" +
                                               " the scientific and technological revolutions of the last 500 years, exposes the artificial " +
                                               "divergence of disciplinary categories",

                                               imageUri: "https://res.cloudinary.com/dfmpdhjz9/image/upload/v1582808895/Schoolman%20course%20seeding%20images/VR/Art_zjx5lj.gif",
                                               duration: TimeSpan.FromHours(30),
                                               skillsDescription: new string[]
                                               {
                                                    "Provide a comprehensive review of the history and theory of art as it is seen in the digital age, informing them of the reciprocal relationship between art, design and technology",
                                                    "Provide students with knowledge of digital tools that are used in today’s developing industry",
                                                    "Provide students with a set of terms and a language that will enable them to assess and analyse contemporary art and design",
                                                    "Introduce students to some of the leading thinkers in the field",
                                               },
                                               courseType: CourseType.VR);

                        #endregion

                        #region Create course on human geography

                        var customer_magellan = await CreateCustomerAsync("Ferdinand", "Magellan");

                        Instructor instructor_magellan = await CreateInstructorAsync(customer_magellan.Id);

                        await CreateCourseAsync(instructor_magellan.Id,
                                               name: "Introduction to AP Human Geography",
                                               description: "Explore key issues in human geography, including population, migration, cultural patterns and more as you prepare for the AP exam.",

                                               about: "By exploring human influences and patterns, you can better understand the world " +
                                               "around you, make predictions, and propose solutions to current issues. In this course, you " +
                                               "will investigate geographic perspectives and analyze historical and current patterns of migration, " +
                                               "population, political organization of space, agriculture, food production, land use, industrialization" +
                                               " and economic development",

                                               imageUri: "https://res.cloudinary.com/dfmpdhjz9/image/upload/v1582808905/Schoolman%20course%20seeding%20images/VR/geography_zv9a9a.gif",
                                               duration: TimeSpan.FromHours(45),
                                               skillsDescription: new string[]
                                               {
                                                    "how to interpret maps and analyze geospatial data",
                                                    "ways to determine implications of associations and networks among phenomena in places",
                                                    "how to recognize relationships among patterns and processes at different scales of analysis",
                                                    "strategies for analyzing interconnections among places",
                                                    "methods to define regions and regionalization processes",
                                               },
                                               courseType: CourseType.VR);



                        #endregion

                        #region Create course on Accounting Essentials

                        var customer_tonny = await CreateCustomerAsync("Tonny", "Robbins");

                        Instructor instructor_tonny = await CreateInstructorAsync(customer_tonny.Id);

                        await CreateCourseAsync(instructor_tonny.Id,
                                               name: "Accounting Essentials",
                                               description: "An introduction to the financial and management accounting skills needed to succeed in both MBA study and in business.",

                                               about: "Want to study for an MBA but unsure of your basic accounting skills? Paving the way for MBA study, this course will" +
                                               " teach you the foundational accounting skills needed to achieve success on an MBA program and in business generally",

                                               imageUri: "https://res.cloudinary.com/dfmpdhjz9/image/upload/v1582808885/Schoolman%20course%20seeding%20images/InClass/accounting_hievmy.gif",
                                               duration: TimeSpan.FromHours(70),
                                               skillsDescription: new string[]
                                               {
                                                    "How to create and interpret the three basic accounting statements - the income statement, the balance sheet and the cash flow statement",
                                                    "How management accounting differs from financial accounting",
                                                    "how to recognize relationships among patterns and processes at different scales of analysis",
                                                    "How financial information is used on an MBA program and in business to make informed decisions"
                                               },
                                               courseType: CourseType.InClass);



                        #endregion

                        #region Create course on Human Microbiome

                        var customer_louis = await CreateCustomerAsync("Louis", "Pasteur");

                        Instructor instructor_louis = await CreateInstructorAsync(customer_louis.Id);

                        await CreateCourseAsync(instructor_louis.Id,
                                               name: "Nutrition and Health: Human Microbiome",
                                               description: "Learn how you can impact your health by balancing your gut health.",

                                               about: "In this course, you will learn how the human microbiome plays an " +
                                               "important role in maintaining normal gut function, digesting certain nutrients, " +
                                               "early life development, behavior and disorders like irritable bowel syndrome (IBS)," +
                                               " obesity and diabetes. You will learn how to distinguish fact from fiction about the " +
                                               "role of the gut microbiota in health and disease",

                                               imageUri: "https://res.cloudinary.com/dfmpdhjz9/image/upload/v1582808916/Schoolman%20course%20seeding%20images/InDemand/microBiology_uhcetp.gif",
                                               duration: TimeSpan.FromHours(100),
                                               skillsDescription: new string[]
                                               {
                                                    "How to study the microbiome",
                                                    "How microbiota impact your health",
                                                    "Healthy ageing and microbiota",
                                                    "How academic knowledge about the microbiome is important forpolicy makers, medical doctors, non-profit organizations and industry",
                                                    "The relation between diet, genes and microbiota"
                                               },
                                               courseType: CourseType.Online);



                        #endregion

                        // Additional courses will be added

                        await transaction.CommitAsync();

                    }


                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }



            }

        }

        #region Private methods

        private async Task<Customer> CreateCustomerAsync(string first, string last)
        {
            var request = new CustomerRegistrationRequest(firstName: first,
                                                           lastName: last,
                                                           email: $"{first[0]}.{last}@example.com".ToLower(),
                                                           password: $"@#grapeWine{DateTime.Now.Ticks}");



            return (await customerManager.CreateAsync(request)).Response ??
                    throw new DatabaseSeedingException<CustomerRegistrationRequest>
                                (request, "Unable to create customer");
        }


        private async Task<Instructor> CreateInstructorAsync(string customerId)
        {
            // create instructor and bind to customer
            var instructor = new Instructor();
            instructor.CustomerId = customerId;
            await AddAndSaveAsync(instructor);
            return instructor;
        }


        private async Task<InstructorCourse> CreateCourseAsync(string instructorId,
                                                               string name,
                                                               string description,
                                                               string imageUri,
                                                               string about,
                                                               TimeSpan duration,
                                                               string[] skillsDescription,
                                                               CourseType courseType)
        {
            // create course 
            var course = new Course(skillsDescription.Count());
            course.ImageUri = imageUri;
            course.Name = name;
            course.Descripton = description;
            course.About = about;
            course.Duration = duration;
            course.Type = courseType;
            await AddAndSaveAsync(course);

            var instructorCourse = new InstructorCourse(course.Id, instructorId);
            await AddAndSaveAsync(instructorCourse);
            return instructorCourse;
        }


        private async Task AddAndSaveAsync<T>(T entity) where T : class
        {
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
        }



        #endregion

    }
}
