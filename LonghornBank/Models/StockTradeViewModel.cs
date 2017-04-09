using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public AppUser StockCustomerProfile { get; set; }

        // The Stock Account Associated 
        public StockAccount AccountStock { get; set; }

        // The Checking Account Assoicated
        public List<Checking> CheckingAccounts { get; set; }

        // The Savings Accounts Assoicated
        public List<Saving> SavingsAccount { get; set; }

        // List to hold available stocks
        public List<StockMarket> StocksForPurchase { get; set; }

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