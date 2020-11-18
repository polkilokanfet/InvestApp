using System.Linq;
using InvestApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InvestApp.Services.DataBaseAccess.Repositories
{
    public class RepositoryCountries : BaseRepository<Country>
    {
        public RepositoryCountries(DbContext context) : base(context)
        {
        }

        public Country GetByName(string name)
        {
            return Find(country => country.Name == name).SingleOrDefault() ?? new Country{ Name = name };
        }
    }
}