using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LonghornBank.Models
{
    public class StockAccount: Account
    {
        public Decimal CashBalance { get; set; }

        public Decimal StockBalance { get; set; }

        public Int32 TradingFee { get; set; }

        public Decimal Gains { get; set; }

        public Decimal Bounses { get; set; }
    }
}