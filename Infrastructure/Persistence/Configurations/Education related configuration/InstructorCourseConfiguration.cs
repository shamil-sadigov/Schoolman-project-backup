using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Configurations.Education_related_configuration
{
    public class InstructorCourseConfiguration : ConfigurationBase<InstructorCourse, int>
    {
        public override void Configure(EntityTypeBuilder<InstructorCourse> builder)
        {
            builder.ToTable("instructor_courses");

            builder.Property(c => c.Status).HasConversion(new EnumToStringConverter<PreparedCourseStatus>());

            builder.HasOne(b => b.Instructor)
                   .WithMany(x=> x.Courses)
                   .HasForeignKey(x => x.InstructorId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Course)
                   .WithMany(x => x.Instructors)
                   .HasForeignKey(x => x.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }


}
