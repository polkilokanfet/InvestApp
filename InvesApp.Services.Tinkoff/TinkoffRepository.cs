using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InvestApp.Domain.Services;
using Tinkoff.Trading.OpenApi.Models;
using Tinkoff.Trading.OpenApi.Network;
using InstrumentType = InvestApp.Domain.Models.InstrumentType;
using Operation = InvestApp.Domain.Models.Operation;

namespace InvesApp.Services.Tinkoff
{
    public class TinkoffRepository : IRepository
    {
        private readonly ISandboxContext _context;

        public TinkoffRepository()
        {
            var connection = ConnectionFactory.GetSandboxConnection("t.6S_c2O6mXa8dkH6aHkZvACVa_jeRyRAR-q_k7zmF-0qbyjW2vEX09a01wCJegeJ6aV38vPfl5jsvw_taUpbuUQ");
            _context = connection.Context;
        }

        public async Task<List<Operation>> GetOperationsAsync()
        {
            var result = new List<Operation>();

            var account = await _context.RegisterAsync(BrokerAccountType.Tinkoff);
            var portfolio = await _context.PortfolioAsync();
            foreach (var position in portfolio.Positions)
            {
                var operations = await _context.OperationsAsync(DateTime.Now.AddDays(-30), DateTime.Now, position.Figi);
                foreach (var operation in operations)
                {
                    result.Add(new Operation()
                    {
                        Figi = operation.Figi,
                        Payment = operation.Payment,
                        Price = operation.Price,
                        Date = operation.Date,
                        InstrumentType = (InstrumentType)((int)operation.InstrumentType)
                    });
                }
            }

            return result;
        }
    }
}
