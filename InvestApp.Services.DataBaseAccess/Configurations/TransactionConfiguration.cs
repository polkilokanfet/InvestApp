using InvestApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvestApp.Services.DataBaseAccess.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasOne(transaction => transaction.Instrument).WithMany();
            builder.HasOne(transaction => transaction.Price);
            builder.HasOne(transaction => transaction.Commission);

            builder.Property(transaction => transaction.Quantity).IsRequired();
            builder.Property(transaction => transaction.Date).IsRequired();
        }
    }
}