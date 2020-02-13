using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Education_related_configuration
{
    public class StudentWishedCoursesConfiguration : ConfigurationBase<StudentWishedCourses, int>
    {
        public override void Configure(EntityTypeBuilder<StudentWishedCourses> builder)
        {
            builder.ToTable("StudentWishedCoursess");

            builder.HasOne(x => x.Student)
                   .WithMany(x => x.CoursesWished)
                   .HasForeignKey(x => x.StudentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Course)
                  .WithMany(x => x.StudentsWished)
                  .HasForeignKey(x => x.CourseId)
                  .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }


}
