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