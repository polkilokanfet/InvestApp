using System.Reflection;
using InvestApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InvestApp.Services.DataBaseAccess
{
    public class InvestAppDbContext : DbContext
    {
        public DbSet<Instrument> Instruments { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<MoneySum> MoneySums { get; set; }
        public DbSet<CompanyProfile> CompanyProfiles { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<FinancialRatio> FinancialRatios { get; set; }
        public DbSet<CompanyRaiting> CompanyRaitings { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<RatingRecommendation> RatingRecommendations { get; set; }
        public DbSet<FinancialModelingServiceInvalidSymbols> FinancialModelingServiceInvalidSymbols { get; set; }

        public InvestAppDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=InvestAppDb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(this.GetType()));
        }
    }
}