using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class CompanyConfiguration : ConfigurationBase<Company, string>
    {
        public override void Configure(EntityTypeBuilder<Company> companyBuilder)
        {
            companyBuilder.ToTable("Companies");
            companyBuilder.Property(model => model.Name).HasMaxLength(100);
            base.Configure(companyBuilder);
        }
    }
}
