using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Configurations.Education_related_configuration
{
    public class CourseConfiguration: ConfigurationBase<Course, int>
    {
        public override void Configure(EntityTypeBuilder<Course> course)
        {
            course.ToTable("courses");
            course.Property(model => model.Name).HasMaxLength(100)
                                                .IsRequired();

            course.Property(model => model.Descripton).IsRequired();

            course.Property(model => model.ImageUri).IsRequired(false);
            course.Property(model => model.ImageUri).HasMaxLength(200);

            // this configuration will store enum in db as a string value not db
            course.Property(c => c.Type).HasConversion(new EnumToStringConverter<CourseType>());
           
            base.Configure(course);
        }
    }
}
