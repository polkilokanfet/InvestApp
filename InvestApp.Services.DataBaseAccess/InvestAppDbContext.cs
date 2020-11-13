using Microsoft.EntityFrameworkCore;

namespace InvestApp.Services.DataBaseAccess
{
    public class InvestAppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=InvestAppDb;Trusted_Connection=True;");
        }
    }
}