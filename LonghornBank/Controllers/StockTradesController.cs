using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LonghornBank.Models;
using System.Net;

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


        // GET: / StockTrades/StockChoices/
        // shows all of the stock choices 
        public ActionResult StockChoices()
        {
            return View();
        }


        // GET: /StockTrades/ViewSelectedStock/
        // Views the selected stock type 
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
        
        public ActionResult ChooseAccount(int? id)
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
            List<StockAccount> CustomerStockAccount = StockAccountQuery.ToList();

            /* Create a None Options 
            StockAccount SelectNoStockAccount = new StockAccount() { StockAccountID = 0, Name = "None", CashBalance = 0 };
            CustomerStockAccount.Add(SelectNoStockAccount);
            */



            // Get the customers checking accounts
            var CheckingQuery = from ca in db.CheckingAccount
                                where ca.Customer.Id == customer.Id
                                select ca;

            // Create a list to hold all of the checking accounts 
            List<Checking> CustomerCheckingAccounts = CheckingQuery.ToList();

            /* Create a None Options 
            Checking SelectNoChecking = new Checking() { CheckingID = 0, Name = "None", Balance = 0 };
            CustomerCheckingAccounts.Add(SelectNoChecking);
            */





            // Get the customers checking accounts
            var SavingsQuery = from sa in db.SavingsAccount
                               where sa.Customer.Id == customer.Id
                               select sa;

            // Create a list to hold all of the checking accounts 
            List<Saving> CustomerSavingsAccounts = SavingsQuery.ToList();

            /* Create a None Options 
            Saving SelectNoSavings = new Saving() { SavingID = 0, Name = "None", Balance = 0 };
            CustomerSavingsAccounts.Add(SelectNoSavings);
            */

            // bind to the model 
            ChooseAcocunt CustomerInfo = new ChooseAcocunt
            {
                AccountStock = CustomerStockAccount,
                CheckingAccounts = CustomerCheckingAccounts,
                SavingsAccount = CustomerSavingsAccounts,
                CustomerProfile = customer,
                StockSelected = db.StockMarket.Find(id)
            };

            return View("ChooseAccount", CustomerInfo);
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
        public ActionResult PurchaseStocks(PurchaseStockTrade PurchcaseTrade)
        {
            // get the customer 
            AppUser Customer = db.Users.Find(PurchcaseTrade.CustomerProfile.Id);

            // Get the stock 
            StockMarket SelectedStock = db.StockMarket.Find(PurchcaseTrade.SelectedStock.StockMarketID);

            // Get the total amount 
            Decimal decTotal = (PurchcaseTrade.Quantity * SelectedStock.StockPrice);

            // create a new transaction list for the trade 
            List<BankingTransaction> TradeTrans = new List<BankingTransaction>();

            // Create a new fee transaction and add it to the list 
            BankingTransaction FeeTrans = new BankingTransaction
            {
                Amount = SelectedStock.Fee,
                BankingTransactionType = BankingTranactionType.Fee,
                Description = ("Fee for purchase of " + SelectedStock.CompanyName),
                TransactionDate = (PurchcaseTrade.TradeDate),
                StockAccount = Customer.StockAccount.FirstOrDefault()
            };

            TradeTrans.Add(FeeTrans);

            // Make the trade happen
            Trade Trade = new Trade()
            {
                Amount = decTotal,
                Quantity = PurchcaseTrade.Quantity,
                Ticker = SelectedStock.Ticker,
                TransactionDate = PurchcaseTrade.TradeDate,
                PricePerShare = SelectedStock.StockPrice,
                TradeType = TradeType.Buy,
                StockAccount = Customer.StockAccount.FirstOrDefault(),
                StockMarket = SelectedStock,
                BankingTransactions = TradeTrans,
                DisputeMessage = "None",
                TransactionDispute = DisputeStatus.Accepted,
                Description = "None",
                CorrectedAmount = 0 
            };



            // check the account nulls 
            if (PurchcaseTrade.CheckingAccounts != null)
            {
                // find the account
                Checking CustomerChecking = db.CheckingAccount.Find(PurchcaseTrade.CheckingAccounts.CheckingID);

                // take the money from the account
                if (CustomerChecking.Balance - decTotal >= 0)
                {
                    // Check to see if the cash balance is enough for the fee
                    if (db.StockAccount.Find(Customer.StockAccount.FirstOrDefault().StockAccountID).CashBalance < SelectedStock.Fee)
                    {
                        ViewBag.Error = "Fee was greater than Stock Cash Balance";
                        return View("PurchaseError");
                    }

                    // create a list to hold the checking account
                    List<Checking> CheckingList = new List<Checking>();
                    CheckingList.Add(CustomerChecking);

                    //Create a new transaction 
                    BankingTransaction CheckingTrans = new BankingTransaction
                    {
                        Amount = decTotal,
                        BankingTransactionType = BankingTranactionType.Withdrawl,
                        CheckingAccount = CheckingList,
                        Description = ("Stock Purchase - Stock Account " + Customer.StockAccount.FirstOrDefault().AccountNumber.ToString()),
                        TransactionDate = PurchcaseTrade.TradeDate,
                        Trade = Trade,
                    };

                    // add the stuff to the database  
                    db.BankingTransaction.Add(FeeTrans);
                    db.SaveChanges();

                    db.Trades.Add(Trade);
                    db.SaveChanges();

                    // Take the money out 
                    db.CheckingAccount.Find(CustomerChecking.CheckingID).Balance -= decTotal;

                    // take out the fee 
                    db.StockAccount.Find(Customer.StockAccount.FirstOrDefault().StockAccountID).CashBalance -= SelectedStock.Fee;

                    db.BankingTransaction.Add(CheckingTrans);
                    db.SaveChanges();
                }

                // HACK
                else
                {
                    ViewBag.Error = "Not Enough Money in Account to Purchase the Stocks";
                    return View("PurchaseError");
                }


                // Any further changes
            }
            else if (PurchcaseTrade.SavingsAccount != null)
            {
                // Get the customer Savings account 
                Saving CustomerSavings = db.SavingsAccount.Find(PurchcaseTrade.SavingsAccount.SavingID);

                // take the money from the account
                if (CustomerSavings.Balance - decTotal >= 0)
                {

                    // Check to see if the cash balance is enough for the fee
                    if (db.StockAccount.Find(Customer.StockAccount.FirstOrDefault().StockAccountID).CashBalance < SelectedStock.Fee)
                    {
                        ViewBag.Error = "Fee was greater than Stock Cash Balance";
                        return View("PurchaseError");
                    }

                    // create a list to hold the checking account
                    List<Saving> SavingsList = new List<Saving>();
                    SavingsList.Add(CustomerSavings);

                    //Create a new transaction 
                    BankingTransaction CheckingTrans = new BankingTransaction
                    {
                        Amount = decTotal,
                        BankingTransactionType = BankingTranactionType.Withdrawl,
                        SavingsAccount = SavingsList,
                        Description = ("Stock Purchase - Stock Account " + Customer.StockAccount.FirstOrDefault().AccountNumber.ToString()),
                        TransactionDate = PurchcaseTrade.TradeDate,
                        Trade = Trade,
                    };

                    // add the stuff to the database  
                    db.BankingTransaction.Add(FeeTrans);
                    db.SaveChanges();

                    db.Trades.Add(Trade);
                    db.SaveChanges();

                    // Take the money out 
                    db.SavingsAccount.Find(CustomerSavings.SavingID).Balance -= decTotal;

                    // take out the fee 
                    db.StockAccount.Find(Customer.StockAccount.FirstOrDefault().StockAccountID).CashBalance -= SelectedStock.Fee;

                    db.BankingTransaction.Add(CheckingTrans);
                    db.SaveChanges();
                }

                // HACK
                else
                {
                    ViewBag.Error = "Not Enough Money in Account to Purchase the Stocks";
                    return View("PurchaseError");
                }
            }
            else if (PurchcaseTrade.AccountStock!= null)
            {
                // Get the customer Savings account 
                StockAccount CustomerStockAccount = db.StockAccount.Find(PurchcaseTrade.AccountStock.StockAccountID);

                // take the money from the account
                if (CustomerStockAccount.CashBalance - decTotal >= 0)
                {
                    // Check to see if the cash balance is enough for the fee
                    if (db.StockAccount.Find(Customer.StockAccount.FirstOrDefault().StockAccountID).CashBalance < SelectedStock.Fee)
                    {
                        ViewBag.Error = "Fee was greater than Stock Cash Balance";
                        return View("PurchaseError");
                    }

                    // create a list to hold the checking account
                    List<StockAccount> StockAccountList = new List<StockAccount>();
                    StockAccountList.Add(CustomerStockAccount);

                    //Create a new transaction 
                    BankingTransaction CheckingTrans = new BankingTransaction
                    {
                        Amount = decTotal,
                        BankingTransactionType = BankingTranactionType.Withdrawl,
                        StockAccount = CustomerStockAccount,
                        Description = ("Stock Purchase - Stock Account " + Customer.StockAccount.FirstOrDefault().AccountNumber.ToString()),
                        TransactionDate = PurchcaseTrade.TradeDate,
                        Trade = Trade,
                    };

                    // add the stuff to the database  
                    db.BankingTransaction.Add(FeeTrans);
                    db.SaveChanges();

                    db.Trades.Add(Trade);
                    db.SaveChanges();

                    // Take the money out 
                    db.StockAccount.Find(CustomerStockAccount.StockAccountID).CashBalance -= decTotal;

                    // take out the fee 
                    db.StockAccount.Find(Customer.StockAccount.FirstOrDefault().StockAccountID).CashBalance -= SelectedStock.Fee;

                    db.BankingTransaction.Add(CheckingTrans);
                    db.SaveChanges();
                }

                // HACK
                else
                {
                    ViewBag.Error = "Not Enough Money in Account to Purchase the Stocks";
                    return View("PurchaseError");
                }
            }

            else
            {
                return HttpNotFound();
            }

            // Add the stuff to the database 

            return View("PurchaseConfirmation");
        }

        // GET: Trades
        // The individual details for each stock trade
        // id = trade id 
        public ActionResult TradeDetails(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            // Find the trade
            Trade CustomerTrade = db.Trades.Find(id);

            if (CustomerTrade == null)
            {
                return HttpNotFound();
            }

            TradeDetails TD = new Models.TradeDetails
            {
                StockAccountID = CustomerTrade.StockAccount.StockAccountID,
                StockMarketID = CustomerTrade.StockMarket.StockMarketID,
                PurchasePrice = CustomerTrade.PricePerShare,
                CurrentPrice = CustomerTrade.StockMarket.StockPrice,
                PriceChange = (CustomerTrade.PricePerShare - CustomerTrade.StockMarket.StockPrice),
                Quantity = CustomerTrade.Quantity,
                TradeID = CustomerTrade.TradeID,
                Gains = ((CustomerTrade.Quantity * CustomerTrade.PricePerShare) - (CustomerTrade.Quantity * CustomerTrade.StockMarket.StockPrice))
            };

            return View(TD);
        }

        //GET: StockTrades/SellStocksOptions
        // Loads the information to sell the stock
        public ActionResult SellStockOptions(int StockSaleID, int StockAccountID, int TradeID)
        {
            // Get the trade 
            Trade CustomerTrade = db.Trades.Find(TradeID);

            SellStockTradeOptions SSTO = new SellStockTradeOptions
            {
                StockAccountID = StockAccountID,
                StockMarketID = StockSaleID,
                CustomerTrade = CustomerTrade
            };

            return View(SSTO);
        }

        // POST: StockTrades/SellStocksOptions
        // Displays the final information before posting the sale
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SellStockOptions([Bind(Include ="StockAccountID,StockMarketID,TradeID, Quantity,SaleDate")]SellStockTradeOptions SSTO)
        {   
            // 
            return View("SellStocks");
        }

        // POST: StockTrades/SellStocks 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SellStocks(int StockSaleID, int StockAccountID, int TradeID)
        {
            // Get the Customer 
            // Query the Database for the logged in user 
            var CustomerQuery = from c in db.Users
                                where c.UserName == User.Identity.Name
                                select c;
            // Get the Customer 
            AppUser customer = CustomerQuery.FirstOrDefault();

            // Get the original trade 
            Trade OriginalTrade = db.Trades.Find(TradeID);

            // Get the Stock that is being sold
            StockMarket StockSale = db.StockMarket.Find(StockSaleID);

            // Get the Stock Account 
            StockAccount CustomerStockAccount = db.StockAccount.Find(StockAccountID);

            


            return View();
        }
    }
}