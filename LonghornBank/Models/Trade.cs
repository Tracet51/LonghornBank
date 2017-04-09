using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LonghornBank.Models;
using System.ComponentModel.DataAnnotations;

namespace LonghornBank.Models

    // Trade represents the transactions that occurs 
    // An account can have multiple trades 
    // Trades have 1 transaction

{   
    public enum TradeType { Buy, Sell}
    public class Trade
    {
        public Int32 TradeID { get; set; }

        [Required(ErrorMessage = "Transaction Dispute Status is Required")]
        [Display(Name = "Transaction Dispute Status")]
        public DisputeStatus TransactionDispute { get; set; }

        [Required(ErrorMessage = "Transaction Date is Required")]
        [Display(Name = "Transaction Date")]
        public DateTime TransactionDate { get; set; }

        [Required(ErrorMessage = "Transaction Amount is Required")]
        [Display(Name = "Transaction Amount")]
        public Decimal Amount { get; set; }

        [Required(ErrorMessage = "Transaction Description is Required")]
        [Display(Name = "Transaction Description")]
        public String Description { get; set; }

        [Required(ErrorMessage = "Dispute Message is Required")]
        [Display(Name = "Dispute Message")]
        public String DisputeMessage { get; set; }

        [Required(ErrorMessage = "Corrected Transaction Amount is Required")]
        [Display(Name = "Corrected Transaction Amount")]
        public Decimal CorrectedAmount { get; set; }

        [Display(Name = "Buy or Sell")]
        [Required(ErrorMessage = "Trade Type is Required")]
        public TradeType TradeType { get; set; }

        [Required(ErrorMessage = "Price Per Share is Required")]
        [Display(Name = "Price Per Share")]
        public Decimal PricePerShare { get; set; }

        [Required(ErrorMessage = "Numebr of Shares is Required")]
        [Display(Name = "Number of Shares")]
        public Int32 Quantity { get; set; }

        [Required(ErrorMessage = "Ticker is Required")]
        [Display(Name = "Ticker")]
        public string Ticker { get; set; }

        /*                         //
        // Navigational Properties //
        //                         //
        //                         */

        // A trade can have 1 transaction! 
        public virtual List<BankingTransaction> BankingTransactions { get; set; }

        // A trade can belong to only 1 stock account
        public virtual StockAccount StockAccount { get; set; }

        // A trade can belong to 1 stock 
        public virtual StockMarket StockMarket { get; set; }






    }
}