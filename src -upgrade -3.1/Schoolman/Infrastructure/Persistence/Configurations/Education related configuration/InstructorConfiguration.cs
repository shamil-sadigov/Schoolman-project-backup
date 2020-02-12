using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Education_related_configuration
{
    public class InstructorConfiguration : ConfigurationBase<Instructor, string>
    {
        public override void Configure(EntityTypeBuilder<Instructor> instructor)
        {
            instructor.ToTable("Instructors");

            instructor.HasOne(i => i.Customer)
                  .WithOne()
                  .HasForeignKey<Instructor>(i=> i.CustomerId)
                  .OnDelete(DeleteBehavior.Cascade);

            base.Configure(instructor);
        }
    }


}
