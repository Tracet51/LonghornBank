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
    public class ManagersController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Managers
        public ActionResult Index()
        {
            return View("Index");
        }
        // Post Index To revise disputed amounts
        [HttpPost]
        public ActionResult Disputes(string BankingTransactionID, string ManagerApprovedAmount)
        {
            Int32 IntBankingTransactionID = Convert.ToInt32(BankingTransactionID);
            decimal ManagerApprovedDecimal = Convert.ToDecimal(ManagerApprovedAmount);
            BankingTransaction editedtransobject = db.BankingTransaction.Find(IntBankingTransactionID);
            if (editedtransobject.CorrectedAmount == ManagerApprovedDecimal)
            {
                ViewBag.ConfirmationMessage = "You have accepted a customer's dispute for transaction ID #" + BankingTransactionID + " and the amount is now $" + ManagerApprovedDecimal;
                editedtransobject.CorrectedAmount = ManagerApprovedDecimal;
                editedtransobject.TransactionDispute = DisputeStatus.Accepted;
                db.SaveChanges();
            }
            if (editedtransobject.CorrectedAmount != ManagerApprovedDecimal)
            {
                ViewBag.ConfirmationMessage = "You have adjusted transaction ID #" + BankingTransactionID + " and the amount is now $" + ManagerApprovedDecimal;
                editedtransobject.CorrectedAmount = ManagerApprovedDecimal;
                editedtransobject.TransactionDispute = DisputeStatus.Adjusted;
                db.SaveChanges();
            }
            if (editedtransobject.Amount== ManagerApprovedDecimal)
            {
                ViewBag.ConfirmationMessage = "You have rejected the dispute for ID #" + editedtransobject.BankingTransactionID + " and the amount is unchanged";
                editedtransobject.CorrectedAmount = ManagerApprovedDecimal;
                editedtransobject.TransactionDispute = DisputeStatus.Adjusted;
                db.SaveChanges();
            }
            List<BankingTransaction> SelectedDisputes = new List<BankingTransaction>();
            SelectedDisputes = db.BankingTransaction.Where(b => b.TransactionDispute == DisputeStatus.Submitted).ToList();
            SelectedDisputes.OrderBy(b => b.TransactionDate);
            db.SaveChanges();
            return View(SelectedDisputes);
        }
        //Get 
        public ActionResult Disputes()
        {
            //var query = from b in db.BankingTransaction
            //          select b;
            //query = query.Where(b => b.TransactionDispute == DisputeStatus.Submitted);
            //List<BankingTransaction> ListOfDisputes = query.ToList();
            //return View("Disputes", ListOfDisputes);
            List<BankingTransaction> SelectedDisputes = new List<BankingTransaction>();
            SelectedDisputes = db.BankingTransaction.Where(b => b.TransactionDispute == DisputeStatus.Submitted).ToList();
            SelectedDisputes.OrderBy(b => b.TransactionDate);
            db.SaveChanges();
            return View(SelectedDisputes);
        }

        [HttpPost]
        public ActionResult DisputeManagementDetail (string BankingTransactionID, string ManagerApprovedAmount)
        {
            Int32 IntBankingTransactionID = Convert.ToInt32(BankingTransactionID);
            BankingTransaction editedtransobject = db.BankingTransaction.Find(IntBankingTransactionID);
            decimal ManagerApprovedDecimal = Convert.ToDecimal(ManagerApprovedAmount);
            ViewBag.ID = BankingTransactionID; if (editedtransobject.CorrectedAmount == ManagerApprovedDecimal)
            {
                ViewBag.ConfirmationMessage = "You have accepted a customer's dispute for transaction ID #" + BankingTransactionID + " and the amount is now $" + ManagerApprovedDecimal;
                editedtransobject.CorrectedAmount = ManagerApprovedDecimal;
                editedtransobject.TransactionDispute = DisputeStatus.Accepted;
                db.SaveChanges();
            }
            if (editedtransobject.CorrectedAmount != ManagerApprovedDecimal)
            {
                ViewBag.ConfirmationMessage = "You have adjusted transaction ID #" + BankingTransactionID + " and the amount is now $" + ManagerApprovedDecimal;
                editedtransobject.CorrectedAmount = ManagerApprovedDecimal;
                editedtransobject.TransactionDispute = DisputeStatus.Adjusted;
                db.SaveChanges();
            }
            if (editedtransobject.Amount == ManagerApprovedDecimal)
            {
                ViewBag.ConfirmationMessage = "You have rejected the dispute for ID #" + editedtransobject.BankingTransactionID + " and the amount is unchanged";
                editedtransobject.CorrectedAmount = ManagerApprovedDecimal;
                editedtransobject.TransactionDispute = DisputeStatus.Adjusted;
                db.SaveChanges();
            }
            
            db.SaveChanges();
            return View(editedtransobject);
        }

        //GET
        public ActionResult DisputeManagementDetail(string BankingTransactionID)
        {
            Int32 IntBankingTransactionID = Convert.ToInt32(BankingTransactionID);
            BankingTransaction originaltransobject = db.BankingTransaction.Find(IntBankingTransactionID);
            return View(originaltransobject);
        }
        public ActionResult DepositApproval()
        {
            return View();
        }
        // GET: Managers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

        // GET: Managers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Managers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ManagerID,FName,LName,StreetAddress,City,State,Zip,EmailAddress,Password,PhoneNumber,SSN,ActiveStatus,FiredStatus")] Manager manager)
        {
            if (ModelState.IsValid)
            {
                db.Managers.Add(manager);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(manager);
        }

        // GET: Managers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

        // POST: Managers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ManagerID,FName,LName,StreetAddress,City,State,Zip,EmailAddress,Password,PhoneNumber,SSN,ActiveStatus,FiredStatus")] Manager manager)
        {
            if (ModelState.IsValid)
            {
                db.Entry(manager).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(manager);
        }

        // GET: Managers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

        // POST: Managers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Manager manager = db.Managers.Find(id);
            db.Managers.Remove(manager);
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
