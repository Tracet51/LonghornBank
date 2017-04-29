using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LonghornBank.Models;
using LonghornBank.Utility;

namespace LonghornBank.Controllers
{
    public class IRAsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: IRAs
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

            // Add the Customer to ViewBag to Access information 
            ViewBag.CustomerAccount = customer;

            // Select The IRA Accounts Associated with this customer 
            var IRAAccountQuery = from IR in db.IRAAccount
                                  where IR.Customer.Id == customer.Id
                                  select IR;

            // Create list and execute the query 
            List<IRA> CustomerIRA = IRAAccountQuery.ToList();

            return View(CustomerIRA);
        }

        // GET: IRAs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IRA ira = db.IRAAccount.Find(id);
            if (ira == null)
            {
                return HttpNotFound();
            }

            // Get the List off all of the Banking Transaction For this Account 
            List<BankingTransaction> IRATransactions = ira.BankingTransactions.ToList();

            // Pass the List to the ViewBag
            ViewBag.IraTransactions = IRATransactions;
            ViewBag.Ranges = SearchTransactions.AmountRange();
            ViewBag.Dates = SearchTransactions.DateRanges();
            return View(ira);
        }

        public ActionResult Search(SearchViewModel TheSearch, Int32 IRAID)
        {
            // get the ira 
            IRA ira = db.IRAAccount.Find(IRAID);

            List<BankingTransaction> IraTransactions = SearchTransactions.Search(db, TheSearch, 3, IRAID);

            ViewBag.IraTransactions = IraTransactions;
            ViewBag.Ranges = SearchTransactions.AmountRange();
            ViewBag.Dates = SearchTransactions.DateRanges();
            return View("Details", ira);
        }

        // GET: IRA/Create
        public ActionResult Create()
        {
            DateTime Restrict1 = new DateTime(1947, 5, 5, 0, 0, 0);
            var CustomerQuery = from c in db.Users
                                where c.UserName == User.Identity.Name
                                select c;

            AppUser customer = CustomerQuery.FirstOrDefault();

            if (customer == null)
            {
                return HttpNotFound();
            }

            if(customer.DOB >= Restrict1)
            {
                if (customer.IRAAccounts.Count() == 0)
                {
                    ViewBag.CustomerID = customer.Id;
                    return View();
                }
            }

            return View("MoreThanOneError");

        }

        // POST: IRAs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IRAID, Balance, Name")] IRA ira)
        {
            var CustomerQuery = from c in db.Users
                                where c.UserName == User.Identity.Name
                                select c;

            AppUser customer = CustomerQuery.FirstOrDefault();

            if (customer == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                ira.RunningTotal = 0 + ira.Balance;
                ira.Customer = customer;

                ira.AccountNumber = Utility.AccountNumber.AutoNumber(db);

                if (ira.Name == null)
                {
                    ira.Name = "Longhorn IRA";
                }

                db.IRAAccount.Add(ira);
                db.SaveChanges();
                return RedirectToAction("Portal", "Home", new { id = customer.Id });
            }

            return View(ira);

        }

        // GET: IRA/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IRA ira = db.IRAAccount.Find(id);
            if (ira == null)
            {
                return HttpNotFound();
            }
            return View(ira);
        }

        // POST: IRAs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IRAID,Name,AccountNumber")] IRA ira)
        {
            if (ModelState.IsValid)
            {
                // Find the CustomerID Associated with the Account
                var IRACustomerQuery = from IR in db.IRAAccount
                                          where IR.IRAID == ira.IRAID
                                          select IR.Customer.Id;


                // Execute the Find
                List<String> CustomerID = IRACustomerQuery.ToList();

                String IntCustomerID = CustomerID[0];

                db.Entry(ira).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Portal", "Home", new { id = IntCustomerID });
            }
            return RedirectToAction("Index", "Checkings", new { id = ira.IRAID });
        }

        // GET: IRAs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IRA ira = db.IRAAccount.Find(id);
            if (ira == null)
            {
                return HttpNotFound();
            }
            return View(ira);
        }

        // POST: IRAs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Find the CustomerID Associated with the Account
            var IRACustomerQuery = from IR in db.IRAAccount
                                      where IR.IRAID == id
                                      select IR.Customer.Id;
           

            // Execute the Find
            List<String> CustomerID = IRACustomerQuery.ToList();

            String IntCustomerID = CustomerID[0];

            IRA ira = db.IRAAccount.Find(id);
            db.IRAAccount.Remove(ira);
            db.SaveChanges();
            return RedirectToAction("Portal", "Home", new { id = IntCustomerID });
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
