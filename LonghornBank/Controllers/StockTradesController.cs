using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LonghornBank.Models;

namespace LonghornBank.Controllers
    // This Controller will handle the displaying of all stocks, purchasing of stocks, and selling of stocks
{
    public class StockTradesController : Controller
    {
        // Connect to the DB
        private AppDbContext db = new AppDbContext();

        // GET: /StockTrades/StocksHome
        // Returns all of the trades associated with the account
        public ActionResult StocksHome()
        {

            // Get the users account 
            // Query the Database for the logged in user 
            var CustomerQuery = from c in db.Users
                                where c.UserName == User.Identity.Name
                                select c;
            // Get the Customer 
            AppUser customer = CustomerQuery.FirstOrDefault();

            // Get the customers stock account 
            // Query the Database for the logged in user 
            var StockAccountQuery = from sa in db.StockAccount
                                    where sa.Customer.Id == customer.Id
                                    select sa;

            // Select the account 
            StockAccount CustomerStockAccount = StockAccountQuery.FirstOrDefault();

            // Get all of the Trades for the account
            var TradesQuery = from t in db.Trades
                            where t.StockAccount.StockAccountID == CustomerStockAccount.StockAccountID
                            select t;

            // Put all of the trades into a list
            List<Trade> CustomerTrades = TradesQuery.ToList();

            // Associate everything with the model 
            // Create a new instance of the StockTrades ViewModel 
            StockTradeViewModel StockTrade = new StockTradeViewModel { StockCustomerProfile = customer,
                AccountStock = CustomerStockAccount, Trades = CustomerTrades };

            return View(StockTrade);
        }

        // GET: /StockTrades/PurchaseStocks
        // Returns page to purchase stocks
        public ActionResult PurchaseStocks()
        {
            // Get the users account 
            // Query the Database for the logged in user 
            var CustomerQuery = from c in db.Users
                                where c.UserName == User.Identity.Name
                                select c;
            // Get the Customer 
            AppUser customer = CustomerQuery.FirstOrDefault();

            // Get the customers stock account 
            // Query the Database for the logged in user 
            var StockAccountQuery = from sa in db.StockAccount
                                    where sa.Customer.Id == customer.Id
                                    select sa;

            // Select the account 
            StockAccount CustomerStockAccount = StockAccountQuery.FirstOrDefault();


            // Get the customers checking accounts
            var CheckingQuery = from ca in db.CheckingAccount
                                where ca.Customer.Id == customer.Id
                                select ca;

            // Create a list to hold all of the checking accounts 
            List<Checking> CustomerCheckingAccounts = CheckingQuery.ToList();

            // Get the customers checking accounts
            var SavingsQuery = from sa in db.SavingsAccount
                                where sa.Customer.Id == customer.Id
                                select sa;

            // Create a list to hold all of the checking accounts 
            List<Saving> CustomerSavingsAccounts = SavingsQuery.ToList();


            // Get all of the stocks for sale 
            var StocksQuery = from s in db.StockMarket
                              select s;

            // List to hold all of the stocks
            List<StockMarket> StocksAvailable = StocksQuery.ToList();


            PurchaseStockTrade StockPurchaseInfo = new PurchaseStockTrade
            {
                StockCustomerProfile = customer,
                SavingsAccount = CustomerSavingsAccounts,
                AccountStock = CustomerStockAccount,
                CheckingAccounts = CustomerCheckingAccounts,
                StocksForPurchase = StocksAvailable
            };

            return View(StockPurchaseInfo);
        }
    }
}