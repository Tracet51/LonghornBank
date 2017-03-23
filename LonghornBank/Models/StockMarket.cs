using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LonghornBank.Models
{   
    public enum StockType { Ordinary, ETF, Future_Share, Mutual_Fund, Index_Fund, Other}
    public class StockMarket
    {
        public Int32 StockMarketID { get; set; }

        public String Name { get; set; }

        public String Ticker { get; set; }

        public StockType StockType { get; set; }

        public Decimal Fee { get; set; }

        public Decimal StockPrice { get; set; }
    }
}