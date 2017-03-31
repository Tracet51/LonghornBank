using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LonghornBank.Models
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

        [Required(ErrorMessage = "A Stock Price is Required")]
        [Display(Name = "Stock Price")]
        public Decimal StockPrice { get; set; }
    }
}