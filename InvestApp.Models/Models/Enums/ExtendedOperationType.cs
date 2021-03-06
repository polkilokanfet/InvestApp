﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace InvestApp.Domain.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExtendedOperationType
    {
        Buy,
        BuyCard,
        Sell,
        BrokerCommission,
        ExchangeCommission,
        ServiceCommission,
        MarginCommission,
        OtherCommission,
        PayIn,
        PayOut,
        Tax,
        TaxLucre,
        TaxDividend,
        TaxCoupon,
        TaxBack,
        Repayment,
        PartRepayment,
        Coupon,
        Dividend,
        SecurityIn,
        SecurityOut
    }
}