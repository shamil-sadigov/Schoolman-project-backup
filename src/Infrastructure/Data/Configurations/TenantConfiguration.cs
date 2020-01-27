using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Schoolman.Student.Core.Domain.Domain;

namespace Schoolman.Student.Infrastructure.Data.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> company)
        {
            company.HasMany(c => c.UserRoleCompanies)
                   .WithOne()
                   .HasForeignKey(c => c.TenantId)
                   .OnDelete(DeleteBehavior.SetNull)
                   .IsRequired();
        }
    }
}
