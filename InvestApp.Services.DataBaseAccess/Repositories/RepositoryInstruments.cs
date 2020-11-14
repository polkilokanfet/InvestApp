using System.Linq;
using InvestApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InvestApp.Services.DataBaseAccess.Repositories
{
    public class RepositoryInstruments : BaseRepository<Instrument>
    {
        public RepositoryInstruments(DbContext context) : base(context)
        {
        }

        //protected override IQueryable<Instrument> GetQuary()
        //{
        //    return base.GetQuary()
        //        .Include(instrument => instrument.Name)
        //        .Include(instrument => instrument.Ticker);
        //}
    }
}