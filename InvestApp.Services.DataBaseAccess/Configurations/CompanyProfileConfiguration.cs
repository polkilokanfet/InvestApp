using InvestApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvestApp.Services.DataBaseAccess.Configurations
{
    public class CompanyProfileConfiguration : IEntityTypeConfiguration<CompanyProfile>
    {
        public void Configure(EntityTypeBuilder<CompanyProfile> builder)
        {
            builder.HasOne(companyProfile => companyProfile.Country).WithMany();
            builder.HasOne(companyProfile => companyProfile.Sector).WithMany();
            builder.HasOne(companyProfile => companyProfile.Industry).WithMany();
            builder.HasOne(companyProfile => companyProfile.Exchange).WithMany();

            builder.OwnsMany(companyProfile => companyProfile.CompanyRaitings, 
                ratingNavigationBuilder =>
                {
                    ratingNavigationBuilder.HasOne(companyRaiting => companyRaiting.Rating).WithMany();
                    ratingNavigationBuilder.HasOne(companyRaiting => companyRaiting.RatingRecommendation).WithMany();
                    ratingNavigationBuilder.HasOne(companyRaiting => companyRaiting.RatingDetailsDCFRecommendation).WithMany();
                    ratingNavigationBuilder.HasOne(companyRaiting => companyRaiting.RatingDetailsROERecommendation).WithMany();
                    ratingNavigationBuilder.HasOne(companyRaiting => companyRaiting.RatingDetailsROARecommendation).WithMany();
                    ratingNavigationBuilder.HasOne(companyRaiting => companyRaiting.RatingDetailsDERecommendation).WithMany();
                    ratingNavigationBuilder.HasOne(companyRaiting => companyRaiting.RatingDetailsPERecommendation).WithMany();
                    ratingNavigationBuilder.HasOne(companyRaiting => companyRaiting.RatingDetailsPBRecommendation).WithMany();
                });
            builder.OwnsMany(companyProfile => companyProfile.FinancialRatios).WithOwner();

        }
    }
}