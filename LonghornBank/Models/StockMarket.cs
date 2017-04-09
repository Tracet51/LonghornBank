using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using YSQ.core.Quotes;

namespace LonghornBank.Models

    // Stock Market Represents the actual stocks that can be traded
{   
    public enum StockType { Ordinary, ETF, Future_Share, Mutual_Fund, Index_Fund, Other}
    public class StockMarket
    {
        public Int32 StockMarketID { get; set; }

        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "A Company Name is Required")]
        public String CompanyName { get; set; }

        [Required(ErrorMessage = "Ticker is Required")]
        [Display(Name = "Ticker")]
        public String Ticker { get; set; }

        [Required(ErrorMessage = "A Stock Type is Required")]
        [Display(Name = "Stock Type")]
        public StockType StockType { get; set; }

        [Required(ErrorMessage = "A Trading Fee is Required")]
        [Display(Name = "Trading Fee")]
        public Decimal Fee { get; set; }

        // Create a backing variable
        private Decimal _decStockPrice;

        [Required(ErrorMessage = "A Stock Price is Required")]
        [Display(Name = "Stock Price")]
        public Decimal StockPrice
        {
            get {

                try
                {
                    var quote_service = new QuoteService();

                    var quotes = quote_service.Quote(this.Ticker.ToUpper()).Return(QuoteReturnParameter.LatestTradePrice);

                    _decStockPrice = Convert.ToDecimal(quotes.LatestTradePrice);
                }
                catch (Exception)
                {

                    _decStockPrice = 0;
                }


                // Return the price
                return _decStockPrice;

            }

            set { _decStockPrice = value; }
        }

        /*                         //
        // Navigational Properties //
        //                         //
        //                         */

        // A stock can belong to multiple trades 
        public virtual List<Trade> Trades { get; set; }


    }
}