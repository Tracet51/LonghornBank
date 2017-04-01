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
    public class CheckingsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Checkings
        public ActionResult Index()
        {   
            
            return View(db.CheckingAccount.ToList());
        }

        // GET: Checkings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Checking checking = db.CheckingAccount.Find(id);
            if (checking == null)
            {
                return HttpNotFound();
            }
            return View(checking);
        }

        // GET: Checkings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Checkings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Balance, Name, AccountNumber")] Checking checking)
        {
            if (ModelState.IsValid)
            {
                db.CheckingAccount.Add(checking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(checking);
        }

        // GET: Checkings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Checking checking = db.CheckingAccount.Find(id);
            if (checking == null)
            {
                return HttpNotFound();
            }
            return View(checking);
        }

        // POST: Checkings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name")] Checking checking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(checking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(checking);
        }

        // GET: Checkings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Checking checking = db.CheckingAccount.Find(id);
            if (checking == null)
            {
                return HttpNotFound();
            }
            return View(checking);
        }

        // POST: Checkings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Checking checking = db.CheckingAccount.Find(id);
            db.CheckingAccount.Remove(checking);
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
