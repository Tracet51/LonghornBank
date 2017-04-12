using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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

    public class SellStockTrade
    {
        // The customer
        public AppUser StockCustomerProfile { get; set; }

        // The Stock Account Associated 
        public StockAccount AccountStock { get; set; }

        // Property to hold all of the trades
        public Trade Trades { get; set; }

        // Hold the transaction for the trade 
        public BankingTransaction Transaction { get; set; }
    }
}