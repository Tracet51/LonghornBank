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
    public class CustomersController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Customers
        public ActionResult Index()
        {
            
            return View(db.Users.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
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
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerID,FName,LName,StreetAddress,City,State,Zip,EmailAddress,Password,PhoneNumber,DOB,ActiveStatus")] AppUser customer)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Portal", "Home", new { id=customer.Id});
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
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
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,FName,LName,StreetAddress,City,State,Zip,EmailAddress,Password,PhoneNumber,DOB,ActiveStatus")] AppUser customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
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
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AppUser customer = db.Users.Find(id);
            db.Users.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddPayee()
        {
            var CustomerQuery = from c in db.Users
                                where c.UserName == User.Identity.Name
                                select c;

            AppUser customer = CustomerQuery.FirstOrDefault();

            if (customer == null)
            {
                return HttpNotFound();
            }

            ViewBag.AllPayees = GetAllPayees();

            return View(customer);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPayee(AppUser customer, Int32 PayeeID1)
        {
            var CustomerQuery = from c in db.Users
                                where c.UserName == User.Identity.Name
                                select c;

            AppUser user = CustomerQuery.FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }

            Payee selectedPayee = db.Payees.Find(PayeeID1);

            List<Payee> PayeeList = new List<Payee>();

            PayeeList.Add(selectedPayee);

            user.PayeeAccounts = new List<Payee>();

            user.PayeeAccounts.Add(selectedPayee);

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("PayBillsPage", "Payees");
        }

        public SelectList GetAllPayees(Payee selectedPayee)
        {
            var query = from p in db.Payees
                        orderby p.Name
                        select p;

            List<Payee> payeeList = query.ToList();

            SelectList PayeeSelectList = new SelectList(payeeList, "PayeeID", "Name");

            return PayeeSelectList;
        }

        public SelectList GetAllPayees()
        {
            var query = from p in db.Payees
                        orderby p.Name
                        select p;

            List<Payee> payeeList = query.ToList();

            SelectList PayeeSelectList = new SelectList(payeeList, "PayeeID", "Name");

            return PayeeSelectList;
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
