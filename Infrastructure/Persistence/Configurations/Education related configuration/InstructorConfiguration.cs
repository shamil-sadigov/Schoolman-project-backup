using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Education_related_configuration
{
    public class InstructorConfiguration : ConfigurationBase<Instructor, string>
    {
        public override void Configure(EntityTypeBuilder<Instructor> instructor)
        {
            instructor.ToTable("instructors");

            instructor.HasOne(i => i.Customer)
                      .WithMany(x=> x.Instructors)
                      .HasForeignKey(i=> i.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict);

            base.Configure(instructor);
        }
    }

}
