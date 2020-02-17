using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Configurations.Education_related_configuration
{
    public class CourseFeeConfiguration: IEntityTypeConfiguration<CourseFee>
    {

        public void Configure(EntityTypeBuilder<CourseFee> courseFee)
        {
            courseFee.ToTable("course_fees");

            courseFee.HasKey(x => x.Id);
            courseFee.Property(x => x.Id).ValueGeneratedOnAdd();

            courseFee.Property(c => c.Currency)
                        .HasConversion(new EnumToStringConverter<CurrencyType>());


            courseFee.HasOne(x => x.Course)
                     .WithOne(x => x.Fee)
                     .HasForeignKey<CourseFee>(c => c.CourseId)
                     .OnDelete(DeleteBehavior.Cascade);

        }
    }


}
