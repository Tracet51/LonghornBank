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


        // GET: Home/Portal
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

            // Send to the Viewbag
            ViewBag.CheckingAccounts = CustomerCheckings;

            // Find the Savings Accounts Associated 
            var SavingsQuery = from sa in db.SavingsAccount
                               where sa.Customer.Id == customer.Id
                               select sa;

            // Convert to a list and execute 
            List<Saving> CustomerSavings = SavingsQuery.ToList();
            ViewBag.SavingsAccounts = CustomerSavings;

            // Find the IRA Accounts Associated
            var IRAQuery = from IR in db.IRAAccount
                               where IR.Customer.Id == customer.Id
                           select IR;

            List<IRA> CustomerIRA = IRAQuery.ToList();
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

                // Add to the view bag 
                ViewBag.StockAccountValue = (CustomerSA.CashBalance + CustomerSA.StockBalance);
            }

            else
            {
                // Add to the view bag 
                ViewBag.StockAccountValue = 0;
            }

            return View(customer);
        }
    }
}