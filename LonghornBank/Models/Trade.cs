using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LonghornBank.Models;

namespace LonghornBank.Models
{   
    public enum TradeType { Buy, Sell}
    public class Trade: Transaction
    {
        public TradeType TradeType { get; set; }

        public Decimal PricePerShare { get; set; }

        public Decimal Quantity { get; set; }

        public string Ticker { get; set; }


    }
}