using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InvestApp.Domain.Exceptions;
using InvestApp.Domain.Models;

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
        Task<double> GetPrice(string symbol);
        Task<MajorIndex> GetMajorIndex(MajorIndexType indexType);
        Task<List<StockListItem>> GetStockList();
    }
}