using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LonghornBank.Models

    // View Model to display the account's trades, and allow purchases and sales of stocks
{
    public class StockTradeViewModel
    {
        
        // The Customer 
        public AppUser StockCustomerProfile { get; set; }

        // The Stock Account Associated with the customer
        public StockAccount AccountStock { get; set; }

        // Property to hold all of the trades
        public List<Trade> Trades { get; set; }


    }

    public class PurchaseStockTrade
    {
        // The customer
        public AppUser CustomerProfile { get; set; }

        // The Stock Account Associated 
        public StockAccount AccountStock { get; set; }

        // The Checking Account Assoicated
        public Checking CheckingAccounts { get; set; }

        // The Savings Accounts Assoicated
        public Saving SavingsAccount { get; set; }

        // The Selected Stock
        public StockMarket SelectedStock { get; set; }

        // Get the Quantity of Stock 
        public Int32 Quantity { get; set; }

        // Date of the Purchase 
        [Required(ErrorMessage = "Trade Date is Required")]
        [Display(Name ="Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TradeDate { get; set; }

    }


    public class ChooseAcocunt
    {
        // Get the User
        public AppUser CustomerProfile { get; set; }

        // The Stock Account Associated 
        public List<StockAccount> AccountStock { get; set; }

        // The Checking Account Assoicated
        public List<Checking> CheckingAccounts { get; set; }

        // The Savings Accounts Assoicated
        public List<Saving> SavingsAccount { get; set; }

        public StockMarket StockSelected { get; set; }
    }

    public class StocksAvailable
    {
        // The customer
        public AppUser CustomerProfile { get; set; }

        // List to hold available stocks
        public List<StockMarket> AvailableStocks { get; set; }
    }

    public class SellStockTradeOptions
    {

        [Required(ErrorMessage = "Quantity is Required")]
        public Int32 Quantity { get; set; }

        // Date of the Purchase 
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime SaleDate { get; set; }

        public Int32 StockMarketID { get; set; }

        public Int32 StockAccountID { get; set; }

        public Int32 TradeID { get; set; }

        public Trade CustomerTrade { get; set; }

    }

    public class SellStocksTrade
    {
        public Int32 QuantitySold { get; set; }

        public String StockName { get; set; }

        public Int32 SharesRemaining { get; set; }

        public Decimal Fee { get; set; }

        public Decimal Profit { get; set; }

        // Date of the Purchase 
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime SaleDate { get; set; }
        public Int32 StockMarketID { get; set; }

        public Int32 StockAccountID { get; set; }

        public Int32 TradeID { get; set; }

    }

    public class TradeDetails
    {
        public Int32 StockMarketID { get; set; }

        public Int32 StockAccountID { get; set; }

        public Int32 TradeID { get; set; }

        [Display(Name = "Quantity")]
        public Decimal Quantity { get; set; }

        [Display(Name = "Purchase Price")]
        public Decimal PurchasePrice { get; set; }

        [Display(Name = "Current Price")]
        public Decimal CurrentPrice { get; set; }

        [Display(Name = "Price Change")]
        public Decimal PriceChange { get; set; }

        [Display(Name = "Total Gains/Losses")]
        public Decimal Gains { get; set; }
    }
}