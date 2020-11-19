using System;
using System.Collections.Generic;
using InvestApp.Domain.Models.Base;

namespace InvestApp.Domain.Models
{
    public class CompanyProfile : BaseEntity
    {
        public string Symbol { get; set; }
        public string CompanyName { get; set; }
        public Currency Currency { get; set; }
        public string Cik { get; set; }
        public string Isin { get; set; }
        public string Cusip { get; set; }
        public string Description { get; set; }
        public Exchange Exchange { get; set; }
        public Sector Sector { get; set; }
        public Industry Industry { get; set; }
        public Country Country { get; set; }
        public string Website { get; set; }
        public string Image { get; set; }
        public DateTime? IpoDate { get; set; }

        public List<CompanyRaiting> CompanyRaitings { get; set; }
        public List<FinancialRatio> FinancialRatios { get; set; }
    }
}