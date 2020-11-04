using System;

namespace InvestApp.Infrastructure.Interfaces
{
    public interface IHasId
    {
        Guid Id { get; }
    }
}