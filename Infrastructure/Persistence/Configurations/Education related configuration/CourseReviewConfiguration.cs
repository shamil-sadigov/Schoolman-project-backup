using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Education_related_configuration
{
    public class CourseReviewConfiguration : ConfigurationBase<CourseReview, string>
    {
        public override void Configure(EntityTypeBuilder<CourseReview> review)
        {
            review.ToTable("CourseReviews");

            review.Property(r => r.Text).IsRequired();

            review.HasOne(r => r.Course)
                  .WithMany(c => c.Reviews)
                  .HasForeignKey(r => r.CourseId)
                  .OnDelete(DeleteBehavior.Cascade);

            review.HasOne(r => r.Student)
                  .WithMany(c => c.Reviews)
                  .HasForeignKey(r => r.StudentId)
                  .OnDelete(DeleteBehavior.SetNull);

            base.Configure(review);
        }
    }


}
