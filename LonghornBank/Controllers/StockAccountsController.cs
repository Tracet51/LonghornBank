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
            ViewBag.Customer = Customer;

            return View(CustomerStockAccount);
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

            if (customer == null)
            {
                return HttpNotFound();
            }

            // Check to see if there is an account 
            // HACK
            if (customer.StockAccount.FirstOrDefault() != null)
            {
                return View("Index");
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

                if (customer == null)
                {
                    return HttpNotFound();
                }

                // Add relate the stock account to the customer 
                stockAccount.Customer = customer;

                db.StockAccount.Add(stockAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
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
            return View(db.StockAccount.Where(i => i.Customer.Id == customer.Id));
        }

        // POST: StockAccounts/Edit
        // Post request to edit the account 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name")] StockAccount stockAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stockAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stockAccount);
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
            return View(db.StockAccount.Where(i => i.Customer.Id == customer.Id));
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

            StockAccount stockAccount = db.StockAccount.Find(customer.Id);
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
