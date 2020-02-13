using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Configurations
{
    public class ConfigurationBase<Entity, EntityKey> : IEntityTypeConfiguration<Entity>
                                        where Entity : class, IEntityBase<EntityKey>
    {
        public virtual void Configure(EntityTypeBuilder<Entity> builder)
        {
            builder.HasKey(model => model.Id);
            builder.Property(model => model.Id).ValueGeneratedOnAdd();
            builder.Property(model => model.Created).ValueGeneratedOnAdd();
            builder.Property(model => model.LastModified).ValueGeneratedOnUpdate();
            builder.HasQueryFilter(model => !model.IsDeleted);
        }
    }
}
