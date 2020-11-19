using System;

namespace InvestApp.Domain.Models.FinMod
{
    public class CompanyProfileFinMod
    {
        public string Symbol { get; set; }
        public string CompanyName { get; set; }
        public Currency Currency { get; set; }
        public string Cik { get; set; }
        public string Isin { get; set; }
        public string Cusip { get; set; }
        public string Description { get; set; }
        public string Exchange { get; set; }
        public string ExchangeShortName { get; set; }
        public string Sector { get; set; }
        public string Industry { get; set; }
        public string Country { get; set; }
        public string Website { get; set; }
        public string Image { get; set; }
        public DateTime? IpoDate { get; set; }
    }
}