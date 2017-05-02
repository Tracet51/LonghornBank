using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LonghornBank.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Host.SystemWeb;
using LonghornBank.Utility;
using System.Data.Entity.Infrastructure;

namespace LonghornBank.Controllers
{
    public class StockAccountsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: StockAccounts
        // Shows the Stock Account for the customer
        public ActionResult Index()
        {
            // Query the Database for the logged in user 
            var CustomerQuery = from c in db.Users
                                where c.UserName == User.Identity.Name
                                select c;
            // Get the Customer 
            AppUser customer = CustomerQuery.FirstOrDefault();

            if (customer == null)
            {
                return HttpNotFound();
            }


            // Return the Stock Account Associated with the customer
            return View(db.StockAccount.Where(i => i.Customer.Id == customer.Id));

        }

        // GET: StockAccounts/Details
        // Shows all of the Trades for the stock account 
        // Takes the Users to the index page of the StockTradeViewModel
        public ActionResult Details()
        {
            // Get the customer 
            var CustomerQuery = from u in db.Users
                                where u.UserName == User.Identity.Name
                                select u;

            AppUser Customer = CustomerQuery.FirstOrDefault();

            // Get the Stock Account 
            var StockAccountQuery = from s in db.StockAccount
                               where s.Customer.Id == Customer.Id
                               select s;

            StockAccount CustomerStockAccount = StockAccountQuery.FirstOrDefault();


            // Get all of the trades for the customer 
            var TradesQuery = from t in db.Trades
                              where t.StockAccount.StockAccountID == CustomerStockAccount.StockAccountID
                              select t;

            List<Trade> Trades = TradesQuery.ToList();

            // Get all of the transactions
            var TransQuery = from t in db.BankingTransaction
                             where t.StockAccount.StockAccountID == CustomerStockAccount.StockAccountID
                             select t;

            List<BankingTransaction> Trans = TransQuery.ToList();

            // Add to view bag
            ViewBag.Trades = Trades;
            ViewBag.Transactions = Trans;
            ViewBag.Ranges = SearchTransactions.AmountRange();
            ViewBag.Dates = SearchTransactions.DateRanges();
            ViewBag.Customer = Customer;

            return View(CustomerStockAccount);
        }

        public ActionResult Search(SearchViewModel TheSearch, Int32 StockAccountID)
        {
            // get the stock account 
            StockAccount CustomerStockAccount = db.StockAccount.Find(StockAccountID);

            // make a list to hold the transactions 
            List<BankingTransaction> StockTransaction = SearchTransactions.Search(db, TheSearch, 4, StockAccountID);

            // Get the customer 
            var CustomerQuery = from u in db.Users
                                where u.UserName == User.Identity.Name
                                select u;

            AppUser Customer = CustomerQuery.FirstOrDefault();

            // Get all of the trades for the customer 
            var TradesQuery = from t in db.Trades
                              where t.StockAccount.StockAccountID == CustomerStockAccount.StockAccountID
                              select t;

            List<Trade> Trades = TradesQuery.ToList();

            // add the stuff to view bag 
            ViewBag.Trades = Trades;
            ViewBag.Customer = Customer;
            ViewBag.Transactions = StockTransaction;
            ViewBag.Ranges = SearchTransactions.AmountRange();
            ViewBag.Dates = SearchTransactions.DateRanges();
            return View("Details", CustomerStockAccount);
        }

        // GET: StockAccounts/Create
        // Page to create the account
        public ActionResult Create()
        {
            // Query the Database for the logged in user 
            var CustomerQuery = from c in db.Users
                                where c.UserName == User.Identity.Name
                                select c;
            // Get the Customer 
            AppUser customer = CustomerQuery.FirstOrDefault();

            //Return frozen view if no go
            if (customer.ActiveStatus == false)
            {
                return View("Frozen");
            }

            if (customer == null)
            {
                return HttpNotFound();
            }

            // Check to see if there is an account 
            // HACK
            if (customer.StockAccount.FirstOrDefault() != null)
            {
                return View("AccountError");
            }

            return View();
        }

        // POST: StockAccounts/Create
        // Post request to create the account
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StockAccountID,CashBalance,Name")] StockAccount stockAccount)
        {
            if (ModelState.IsValid)
            {
                // Query the Database for the logged in user 
                var CustomerQuery = from c in db.Users
                                    where c.UserName == User.Identity.Name
                                    select c;
                // Get the Customer 
                AppUser customer = CustomerQuery.FirstOrDefault();
                //Change field to needs approval
                stockAccount.ApprovalStatus = ApprovedorNeedsApproval.NeedsApproval;
                if (customer == null)
                {
                    return HttpNotFound();
                }

                // Add relate the stock account to the customer 
                stockAccount.Customer = customer;

                // Get the account number 
                stockAccount.AccountNumber = AccountNumber.AutoNumber(db);

                if (stockAccount.Name == null)
                {
                    stockAccount.Name = "Longhorn Stock Portfolio";
                }

                // set the stock account to needs approval
                stockAccount.ApprovalStatus = ApprovedorNeedsApproval.NeedsApproval;

                db.StockAccount.Add(stockAccount);
                db.SaveChanges();

                // Add the inital deposit to the account 

                // Get the StockAccount
                var StockAccountQuery = from sa in db.StockAccount
                                    where sa.Customer.Id == customer.Id
                                    select sa;

                StockAccount CustomerStockAccount = StockAccountQuery.FirstOrDefault();

                // check to see if the deposit amount is over $5000
                ApprovedorNeedsApproval FirstDeposit;
                if (stockAccount.CashBalance > 5000m)
                {
                    FirstDeposit = ApprovedorNeedsApproval.NeedsApproval;
                }
                else
                {
                    FirstDeposit = ApprovedorNeedsApproval.Approved;
                }

                // Create a new transaction 
                BankingTransaction InitialDeposit = new BankingTransaction
                {
                    Amount = stockAccount.CashBalance,
                    ApprovalStatus = FirstDeposit,
                    BankingTransactionType = BankingTranactionType.Deposit,
                    Description = "Initial Deposit to Cash Balance",
                    TransactionDate = DateTime.Today,
                    TransactionDispute = DisputeStatus.NotDisputed,
                    StockAccount = CustomerStockAccount
                };

                // Add the transaction to the database
                db.BankingTransaction.Add(InitialDeposit);
                db.SaveChanges();

                return View("AccountConfirmation");
            }

            return View(stockAccount);
        }

        // GET: StockAccounts/Edit
        // Get request to see information that they can edit on the account
        public ActionResult Edit()
        {
            // Query the Database for the logged in user 
            var CustomerQuery = from c in db.Users
                                where c.UserName == User.Identity.Name
                                select c;
            // Get the Customer 
            AppUser customer = CustomerQuery.FirstOrDefault();

            if (customer == null)
            {
                return HttpNotFound();
            }

            // Find the stock account associated with the customer 
            // Return the Stock Account Associated with the customer
            StockAccount CustomerStockAccount = db.StockAccount.Where(i => i.Customer.Id == customer.Id).FirstOrDefault();
            return View(CustomerStockAccount);
        }

        // POST: StockAccounts/Edit
        // Post request to edit the account 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StockAccount stockAccount)
        {
            // get the stock account 
            StockAccount CustomerProtfolio = db.StockAccount.Find(stockAccount.StockAccountID);

            CustomerProtfolio.Name = stockAccount.Name;

            db.Entry(CustomerProtfolio).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");   

        }

        // GET: StockAccounts/Delete
        // Pull up the page to delete the Stock Account
        public ActionResult Delete()
        {
            // Query the Database for the logged in user 
            var CustomerQuery = from c in db.Users
                                where c.UserName == User.Identity.Name
                                select c;
            // Get the Customer 
            AppUser customer = CustomerQuery.FirstOrDefault();

            if (customer == null)
            {
                return HttpNotFound();
            }

            // Find the stock account associated with the customer 
            // Return the Stock Account Associated with the customer
            StockAccount CustomerStockAccount = customer.StockAccount.FirstOrDefault();
            return View(CustomerStockAccount);
        }

        // POST: StockAccounts/Delete
        // Actually commit the delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed()
        {
            // Query the Database for the logged in user 
            var CustomerQuery = from c in db.Users
                                where c.UserName == User.Identity.Name
                                select c;
            // Get the Customer 
            AppUser customer = CustomerQuery.FirstOrDefault();

            StockAccount stockAccount = db.StockAccount.Find(customer.StockAccount.FirstOrDefault().StockAccountID);

            // Remove all dependencies
            stockAccount.Trades.RemoveAll(i => i.StockAccount.StockAccountID == stockAccount.StockAccountID);
            stockAccount.BankingTransaction.RemoveAll(i => i.StockAccount.StockAccountID == stockAccount.StockAccountID);

            db.StockAccount.Remove(stockAccount);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
