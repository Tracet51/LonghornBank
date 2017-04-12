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





        // GET: /StockTrades/ViewSelectedStock/
        public ActionResult ViewSelectedStock(int? id)
        {
            // Get the users account 
            // Query the Database for the logged in user 
            var CustomerQuery = from c in db.Users
                                where c.UserName == User.Identity.Name
                                select c;
            // Get the Customer 
            AppUser customer = CustomerQuery.FirstOrDefault();


            // List to hold all of the stocks
            List<StockMarket> StocksAvailable = new List<StockMarket>();

            switch (id)
            {
                case 1:
                    // Get all of the stocks for sale 
                    var OrdinaryQuery = from s in db.StockMarket
                                        where s.StockType == StockType.Ordinary
                                        select s;

                    // List to hold all of the stocks
                    StocksAvailable = OrdinaryQuery.ToList();

                    break;
                case 2:
                    // Get all of the stocks for sale 
                    var MutualQuery = from s in db.StockMarket
                                      where s.StockType == StockType.Mutual_Fund
                                      select s;

                    // List to hold all of the stocks
                    StocksAvailable = MutualQuery.ToList();
                    break;
                case 3:
                    // Get all of the stocks for sale 
                    var IndexQuery = from s in db.StockMarket
                                     where s.StockType == StockType.Index_Fund
                                     select s;

                    // List to hold all of the stocks
                    StocksAvailable = IndexQuery.ToList();
                    break;
                case 4:
                    // Get all of the stocks for sale 
                    var ETFQuery = from s in db.StockMarket
                                   where s.StockType == StockType.ETF
                                   select s;

                    // List to hold all of the stocks
                    StocksAvailable = ETFQuery.ToList();
                    break;
                case 5:
                    // Futures 
                    var FuturesQuery = from s in db.StockMarket
                                       where s.StockType == StockType.Future_Share
                                       select s;

                    StocksAvailable = FuturesQuery.ToList();
                    break;
                case 6:
                    var OtherQuery = from s in db.StockMarket
                                     where s.StockType == StockType.Other
                                     select s;

                    StocksAvailable = OtherQuery.ToList();
                    break;
                default:
                    return HttpNotFound();
                    break;
            }


            StocksAvailable StockPurchaseInfo = new StocksAvailable
            {
                AvailableStocks = StocksAvailable,
                CustomerProfile = customer

            };

            return View(StockPurchaseInfo);
        }


        // GET: /StockTrade/ChooseAccount
        // Once the user has selected the stock they want, they will now select the acocunt
        public ActionResult ChooseAccount(int? StockID)
        {
            // Find the Selected Stock 
            StockMarket SelectedStock = db.StockMarket.Find(StockID);

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
            List<StockAccount> CustomerStockAccount = StockAccountQuery.ToList();

            // Create a None Options 
            StockAccount SelectNoStockAccount = new StockAccount() { StockAccountID = 0, Name = "None", CashBalance = 0 };
            CustomerStockAccount.Add(SelectNoStockAccount);






            // Get the customers checking accounts
            var CheckingQuery = from ca in db.CheckingAccount
                                where ca.Customer.Id == customer.Id
                                select ca;

            // Create a list to hold all of the checking accounts 
            List<Checking> CustomerCheckingAccounts = CheckingQuery.ToList();

            // Create a None Options 
            Checking SelectNoChecking = new Checking() { CheckingID = 0, Name = "None", Balance = 0 };
            CustomerCheckingAccounts.Add(SelectNoChecking);





            // Get the customers checking accounts
            var SavingsQuery = from sa in db.SavingsAccount
                               where sa.Customer.Id == customer.Id
                               select sa;

            // Create a list to hold all of the checking accounts 
            List<Saving> CustomerSavingsAccounts = SavingsQuery.ToList();

            // Create a None Options 
            Saving SelectNoSavings = new Saving() { SavingID = 0, Name = "None", Balance = 0 };
            CustomerSavingsAccounts.Add(SelectNoSavings);

            // bind to the model 
            ChooseAcocunt CustomerInfo = new ChooseAcocunt
            {
                AccountStock = CustomerStockAccount,
                CheckingAccounts = CustomerCheckingAccounts,
                SavingsAccount = CustomerSavingsAccounts,
                CustomerProfile = customer,
                StockSelected = SelectedStock
            };

            return View(CustomerInfo);
        }

        // GET: /StockTrades/PurchaseStocks
        // Returns page to purchase stocks
        public ActionResult PurchaseStocks(int? StockID, int? Choice, int? AccountID)
        {
            // Check to see if any IDs are null 
            if (StockID == null || Choice == null || AccountID == null)
            {
                return HttpNotFound();
            }

            // Get the users account 
            // Query the Database for the logged in user 
            var CustomerQuery = from c in db.Users
                                where c.UserName == User.Identity.Name
                                select c;
            // Get the Customer 
            AppUser customer = CustomerQuery.FirstOrDefault();

            // Get the Selected Stock
            StockMarket SelectedStock = db.StockMarket.Find(StockID);

            // Create a new model instance 
            PurchaseStockTrade StockTrade = new PurchaseStockTrade
            {
                CustomerProfile = customer,
                SelectedStock = SelectedStock
            };

            switch (Choice)
            {
                case 1:
                    // Find the Checking Account
                    Checking SelectedChecking = db.CheckingAccount.Find(AccountID);

                    // Add to model
                    StockTrade.CheckingAccounts = SelectedChecking;

                    break;
                case 2:
                    // Find the savings account
                    Saving SelectedSavings = db.SavingsAccount.Find(AccountID);

                    StockTrade.SavingsAccount = SelectedSavings;

                    break;
                case 3:
                    // Find the portfolio
                    StockAccount SelectedStockAccount = db.StockAccount.Find(AccountID);

                    StockTrade.AccountStock = SelectedStockAccount;

                    break;
                default:
                    return HttpNotFound();
                    break;
            }

            return View(StockTrade);
        }

        // POST /StockTrades/PurchaseStocks
        // Post the trade to the respective accounts
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PurchaseStocks(PurchaseStockTrade Trade)
        {

            // check the account nulls 
            if (Trade.CheckingAccounts != null)
            {
                // Make the trade happen

                // post the transaction

                // take the money from the account
                
                // update the stock account 

                // Any further changes
            }
            else if (Trade.SavingsAccount != null)
            {

            }
            else if (Trade.SelectedStock != null)
            {

            }

            else
            {
                return HttpNotFound();
            }


            return RedirectToAction("Index");
        }

    }
}