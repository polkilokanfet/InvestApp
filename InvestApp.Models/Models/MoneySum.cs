﻿using InvestApp.Domain.Models.Base;

namespace InvestApp.Domain.Models
{
    public class MoneySum : BaseEntity
    {
        public Currency Currency { get; set; }
        public decimal Value { get; set; }
    }
}