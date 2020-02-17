using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Education_related_configuration
{
    public class StudentConfiguration : ConfigurationBase<Student, string>
    {
        public override void Configure(EntityTypeBuilder<Student> course)
        {
            course.ToTable("students");

            course.HasOne(s => s.Customer)
                  .WithOne()
                  .HasForeignKey<Student>(s => s.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);

            base.Configure(course);
        }
    }
}
