using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LonghornBank.Models;

namespace LonghornBank.Controllers
{
    public class HomeController : Controller
    {
        // App Db context 
        private AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            return View();
        }

        // GET: Home/Create 
        // Page where the newly created user chooses what account to make
        public ActionResult Create(string id)
        {
            AppUser customer = db.Users.Find(id);
            return View(customer);
        }

        [Authorize]
        // GET: Home/Portal
        [Authorize]
        public ActionResult Portal()
        {
            var CustomerQuery = from c in db.Users
                                where c.UserName == User.Identity.Name
                                select c;


            // Get the Customer 
            AppUser customer = CustomerQuery.FirstOrDefault();

            if (customer == null)
            {
                return HttpNotFound();
            }

            // Find the Checking Accounts Associated
            var CheckingQuery = from ca in db.CheckingAccount
                                where ca.Customer.Id == customer.Id
                                select ca;

            // Convert to a list and execute
            List <Checking> CustomerCheckings = CheckingQuery.ToList();

            // Loop through each checking account and change the account numbers
            // Only include the last 4 digits
            foreach (Checking account in CustomerCheckings)
            {
                account.AccountNumber = account.AccountNumber.Substring(6);
            }

            foreach (Checking account in CustomerCheckings)
            {
                if(account.Balance < 0)
                {
                    account.Overdrawn = true;
                }
            }

            // Send to the Viewbag
            ViewBag.CheckingAccounts = CustomerCheckings;

            // Find the Savings Accounts Associated 
            var SavingsQuery = from sa in db.SavingsAccount
                               where sa.Customer.Id == customer.Id
                               select sa;

            // Convert to a list and execute 
            List<Saving> CustomerSavings = SavingsQuery.ToList();

            // Loop through and extract only the last 4 digits of account number 
            foreach (Saving account in CustomerSavings)
            {
                account.AccountNumber = account.AccountNumber.Substring(6);
            }

            ViewBag.SavingsAccounts = CustomerSavings;

            foreach (Saving account in CustomerSavings)
            {
                if(account.Balance < 0)
                {
                    account.Overdrawn = true;
                }
            }

            // Find the IRA Accounts Associated
            var IRAQuery = from IR in db.IRAAccount
                               where IR.Customer.Id == customer.Id
                           select IR;

            List<IRA> CustomerIRA = IRAQuery.ToList();

            // Loop through and set the account number to last 4 digits 
            foreach (IRA account in CustomerIRA)
            {
                account.AccountNumber = account.AccountNumber.Substring(6);
            }

            foreach (IRA account in CustomerIRA)
            {
                if(account.Balance < 0)
                {
                    account.Overdrawn = true;
                }
            }

            ViewBag.IRAAccounts = CustomerIRA;

            // Get the total value of the stock porfolio 
            // Get the stock account
            var StockQuery = from sa in db.StockAccount
                             where sa.Customer.Id == customer.Id
                             select sa;

            StockAccount CustomerSA = StockQuery.FirstOrDefault();

            if (CustomerSA != null)
            {
                /* Variable to hold the total amount 
                Decimal StockAccountValue = 0;

                foreach (Trade t in CustomerSA.Trades.Where(i => i.TradeType == TradeType.Buy))
                {
                    StockAccountValue += (t.Quantity * t.PricePerShare);
                }

                StockAccountValue += CustomerSA.CashBalance;

                */

                if(CustomerSA.CashBalance < 0)
                {
                    CustomerSA.Overdrawn = true;
                }

                CustomerSA.AccountNumber = CustomerSA.AccountNumber.Substring(6);

                // Add to the view bag 
                ViewBag.StockAccountValue = (CustomerSA.CashBalance + CustomerSA.StockBalance);
            }

            else
            {
                // Add to the view bag 
                ViewBag.StockAccountValue = 0;
            }

            // check to make the customer has created an account 
            if (CustomerCheckings.Count == 0 && CustomerIRA.Count == 0 && CustomerSavings.Count == 00 && CustomerSA == null)
            {
                // redirect the user to the create an account page
                return RedirectToAction("Create", "Home");
            }

            return View(customer);
        }
    }
}