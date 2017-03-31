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

        // GET: Checkings/CreateTransaction
        public ActionResult Create()
        {
            return RedirectToAction("Create","BankingTransactions");
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
        public ActionResult Edit([Bind(Include = "CheckingID,Balance,PendingBalance,Name")] Checking checking)
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

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deposit([Bind(Include = "BankingTransactionID,TransactionDispute,TransactionDate,Amount,Description,DisputeMessage,CustomerOpinion,CorrectedAmount,BankingTransactionType")] BankingTransaction bankingTransaction, Int32 CheckingID)
        {
            // Find the selected Checking Account
            Checking SelectedChecking = db.CheckingAccount.Find(CheckingID);

            // Associate the transaction with the checking account
            bankingTransaction.CheckingAccount.Add(SelectedChecking);
            
            // Check to see if the model state if valid
            if (ModelState.IsValid)
            {
                db.BankingTransaction.Add(bankingTransaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Repopulate the dropdown options
            ViewBag.AllAccounts = bankingTransaction.CheckingAccount.ToList();
            return View(bankingTransaction);
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
