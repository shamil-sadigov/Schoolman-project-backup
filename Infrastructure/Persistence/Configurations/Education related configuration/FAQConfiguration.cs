using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Education_related_configuration
{
    public class FAQConfiguration : ConfigurationBase<FAQ, int>
    {
        public override void Configure(EntityTypeBuilder<FAQ> faq)
        {
            faq.ToTable("FAQs");

            faq.Property(f => f.Question).IsRequired().HasMaxLength(200);
            faq.Property(f => f.Answer).IsRequired().HasMaxLength(200);

            faq.HasOne(f=> f.Course)
                  .WithMany(c=> c.FAQs)
                  .HasForeignKey(f=> f.CourseId)
                  .OnDelete(DeleteBehavior.Cascade);

            base.Configure(faq);
        }
    }


}
