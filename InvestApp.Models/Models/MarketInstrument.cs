﻿namespace InvestApp.Domain.Models
{
    public class MarketInstrument
    {
        public string Figi { get; set; }
        public string Ticker { get; set; }
        public string Isin { get; set; }
        /// <summary>
        /// Шаг цены
        /// </summary>
        public decimal MinPriceIncrement { get; set; }
        public int Lot { get; set; }
        public Currency Currency { get; set; }
        public string Name { get; set; }
        public InstrumentType Type { get; set; }

        public override string ToString()
        {
            return $"{nameof(Figi)}: {Figi}, {nameof(Ticker)}: {Ticker}, {nameof(Isin)}: {Isin}, {nameof(MinPriceIncrement)}: {MinPriceIncrement}, {nameof(Lot)}: {Lot}, {nameof(Currency)}: {Currency}, {nameof(Name)}: {Name}, {nameof(Type)}: {Type}";
        }
    }
}