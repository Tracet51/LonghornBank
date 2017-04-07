using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LonghornBank.Dal;
using LonghornBank.Models;

namespace LonghornBank.Controllers
{
    public class IRAsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: IRAs
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.CustomerAccount.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            // Add the Customer to ViewBag to Access information 
            ViewBag.CustomerAccount = customer;

            // Select The Savings Accounts Associated with this customer 
            var IRAAccountQuery = from IR in db.IRAAccount
                                  where IR.Customer.CustomerID == customer.CustomerID
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
            ViewBag.IRATransactions = IRATransactions;

            return View(ira);
        }

        // GET: IRA/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.CustomerAccount.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = id;
            return View();
        }

        // POST: IRAs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IRAID, Balance, Name, AccountNumber, Customer_CustomerID")] IRA ira, int? CustomerID)
        {

            if (CustomerID == null)
            {
                return HttpNotFound();
            }

            Customer customer = db.CustomerAccount.Find(CustomerID);
            if (customer == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                ira.Customer = customer;

                db.IRAAccount.Add(ira);
                db.SaveChanges();
                return RedirectToAction("Portal", "Home", new { id = 1 });
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
                                          select IR.Customer.CustomerID;


                // Execute the Find
                List<Int32> CustomerID = IRACustomerQuery.ToList();

                Int32 IntCustomerID = CustomerID[0];

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
                                      select IR.Customer.CustomerID;

            // Execute the Find
            List<Int32> CustomerID = IRACustomerQuery.ToList();

            Int32 IntCustomerID = CustomerID[0];

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
