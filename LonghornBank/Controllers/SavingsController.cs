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
    public class SavingsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Savings
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser customer = db.Users.Find(id);
            if (customer == null)
            {
                return HttpNotFound();

            }

            // Add the Customer to ViewBag to Access information 
            ViewBag.CustomerAccount = customer;

            // Select The Savings Accounts Associated with this customer 
            var SavingAccountQuery = from sa in db.SavingsAccount
                                       where sa.Customer.Id == customer.Id
                                       select sa;

            // Create list and execute the query 
            List<Saving> CustomerSaving = SavingAccountQuery.ToList();

            return View(CustomerSaving);
        }

        // GET: Savings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saving saving = db.SavingsAccount.Find(id);
            if (saving == null)
            {
                return HttpNotFound();
            }

            // Get the List off all of the Banking Transaction For this Account 
            List<BankingTransaction> SavingTransactions = saving.BankingTransactions.ToList();

            // Pass the List to the ViewBag
            ViewBag.SavingTransactions = SavingTransactions;

            return View(saving);
        }

        // GET: Savings/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser customer = db.Users.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = id;
            return View();
        }

        // POST: Savings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SavingID, Balance, Name, AccountNumber, Customer_CustomerID")] Saving saving, int? CustomerID)
        {
            if (CustomerID == null)
            {
                return HttpNotFound();
            }

            AppUser customer = db.Users.Find(CustomerID);
            if (customer == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                saving.Customer = customer;

                db.SavingsAccount.Add(saving);
                db.SaveChanges();
                return RedirectToAction("Portal", "Home", new { id = 1 });
            }

            return View(saving);
        }

        // GET: Savings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saving saving = db.SavingsAccount.Find(id);
            if (saving == null)
            {
                return HttpNotFound();
            }
            return View(saving);
        }

        // POST: Savings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SavingID, Name, AccountNumber")] Saving saving)
        {
            if(ModelState.IsValid)
            {
                // Find the CustomerID Associated with the Account
                var SavingCustomerQuery = from sa in db.SavingsAccount
                                            where sa.SavingID == saving.SavingID
                                            select sa.Customer.Id;


                // Execute the Find
                List<String> CustomerID = SavingCustomerQuery.ToList();

                String IntCustomerID = CustomerID[0];

                db.Entry(saving).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Portal", "Home", new { id = IntCustomerID });
            }
            return RedirectToAction("Index", "Checkings", new { id = saving.SavingID });
        }

        // GET: Savings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saving saving = db.SavingsAccount.Find(id);
            if (saving == null)
            {
                return HttpNotFound();
            }
            return View(saving);
        }

        // POST: Savings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Find the CustomerID Associated with the Account
            var SavingCustomerQuery = from sa in db.SavingsAccount
                                        where sa.SavingID == id
                                        select sa.Customer.Id;

            // Execute the Find
            List<String> CustomerID = SavingCustomerQuery.ToList();

            String IntCustomerID = CustomerID[0];

            Saving saving = db.SavingsAccount.Find(id);
            db.SavingsAccount.Remove(saving);
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
