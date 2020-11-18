using InvestApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvestApp.Services.DataBaseAccess.Configurations
{
    public class CompanyProfileConfiguration : IEntityTypeConfiguration<CompanyProfile>
    {
        public void Configure(EntityTypeBuilder<CompanyProfile> builder)
        {
            builder.HasOne(x => x.Country).WithMany();
            builder.HasOne(x => x.Sector).WithMany();
            builder.HasOne(x => x.Industry).WithMany();
            builder.HasOne(x => x.Exchange).WithMany();

            builder.HasMany(x => x.FinancialRatios).WithOne();
        }
    }
}