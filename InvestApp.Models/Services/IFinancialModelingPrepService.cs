using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InvestApp.Domain.Exceptions;
using InvestApp.Domain.Models;
using InvestApp.Domain.Models.FinMod;

namespace InvestApp.Domain.Services
{
    public interface IFinancialModelingPrepService
    {
        /// <summary>
        /// Get the share price for a symbol.
        /// </summary>
        /// <param name="symbol">The symbol to get the price of.</param>
        /// <returns>The price of symbol.</returns>
        /// <exception cref="InvalidSymbolException">Thrown if symbol does not exist.</exception>
        /// <exception cref="Exception">Thrown if getting the symbol fails.</exception>
        Task<double> GetPriceAsync(string symbol);
        Task<MajorIndex> GetMajorIndexAsync(MajorIndexType indexType);
        Task<List<StockListItem>> GetStockListAsync();
        Task<CompanyProfileFinMod> GetCompanyProfileAsync(string symbol);
        Task<List<FinancialRatio>> GetFinancialRatiosAsync(string symbol);
    }
}