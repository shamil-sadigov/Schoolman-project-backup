using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> tenant)
        {
            tenant.ToTable("Tenants");
            tenant.Property(model => model.Name).HasMaxLength(100);
            tenant.Property(model => model.Id).ValueGeneratedOnAdd();
        }
    }
}
