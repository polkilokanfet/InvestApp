using System;

namespace InvestApp.Domain.Interfaces
{
    public interface IHasId
    {
        Guid Id { get; }
    }
}