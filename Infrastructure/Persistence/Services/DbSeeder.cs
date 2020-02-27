using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Services
{
    public class DbSeeder
    {
        private readonly SchoolmanContext schoolmanContext;

        public DbSeeder(SchoolmanContext schoolmanContext)
        {
            this.schoolmanContext = schoolmanContext;
        }


        public void Seed()
        {

            

        }
    }
}
