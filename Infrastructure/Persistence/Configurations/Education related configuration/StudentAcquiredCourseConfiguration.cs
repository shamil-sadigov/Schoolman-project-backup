using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Configurations.Education_related_configuration
{
    public class StudentAcquiredCourseConfiguration : ConfigurationBase<StudentAcquiredCourse, int>
    {
        public override void Configure(EntityTypeBuilder<StudentAcquiredCourse> builder)
        {
            builder.ToTable("StudentAcquiredCourses");

            builder.Property(c => c.Status).HasConversion(new EnumToStringConverter<AcquiredCourseStatus>());

            builder.HasOne(x => x.Student)
                   .WithMany(x => x.CoursesAcquired)
                   .HasForeignKey(x => x.StudentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Course)
                  .WithMany(x => x.StudentsAcquired)
                  .HasForeignKey(x => x.CourseId)
                  .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }


}
