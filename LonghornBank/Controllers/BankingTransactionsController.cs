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
using System.Diagnostics;

namespace LonghornBank.Controllers
{
    public class BankingTransactionsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: BankingTransactions
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

            // Get a List of checking accounts associated with Customer ID
            List<Checking> CustomerChecking = customer.CheckingAccounts;

            // Create Empty List of Transactions 
            List<BankingTransaction> CustomerTransactions = new List<BankingTransaction>();

            foreach (Checking c in CustomerChecking)
            {
                // Get the Checking Accounts ID
                Int32 inttemp = c.CheckingID;

                // Select the transactions where the checking Id matches the customers checking Id
                var query = from a in db.BankingTransaction
                            from b in a.CheckingAccount
                            where b.CheckingID == inttemp
                            select a;

                // Create a holding list and add to main list
                List<BankingTransaction> TempTransaction = query.ToList();
                CustomerTransactions.AddRange(TempTransaction);
            }

            // Add the customer to the view bag
            ViewBag.Customer = customer;

            return View("Index", CustomerTransactions);
        }

        // GET: BankingTransactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankingTransaction bankingTransaction = db.BankingTransaction.Find(id);
            if (bankingTransaction == null)
            {
                return HttpNotFound();
            }
            return View(bankingTransaction);
        }

        // GET: BankingTransactions/Create/?id
        // id = CustomerID
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

            // Get all of the accounts
            SelectList CheckingAccounts = GetCheckingAccounts((Int32)id);

            // Add the SelectList Tuple to the ViewBag
            ViewBag.CheckingAccounts = CheckingAccounts;

            return View();
        }

        // POST: BankingTransactions/Create
        // CheckingID = the checking account to bind to 
        // id = The Customer's Account ID
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BankingTransactionID,TransactionDate,Amount,Description,BankingTransactionType")] BankingTransaction bankingTransaction,
            int id, int CheckingID)
        {
            if (ModelState.IsValid)
            {
                // Find the Assoicated Account 
                if (CheckingID == 0)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Debug.WriteLine(CheckingID.ToString());

                    // Find the Selected Checking Account
                    Checking SelectedChecking = db.CheckingAccount.Find(CheckingID);

                    // Create a list of checking accounts and add the one seleceted 
                    List<Checking> NewCheckingAccounts = new List<Checking> { SelectedChecking };

                    bankingTransaction.CheckingAccount = NewCheckingAccounts;

                    // Add to database
                    db.BankingTransaction.Add(bankingTransaction);
                    db.SaveChanges();
                }

                
                return RedirectToAction("Index", "BankingTransactions", new { id=id});
            }

            return View(bankingTransaction);
        }

        // GET: BankingTransactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankingTransaction bankingTransaction = db.BankingTransaction.Find(id);
            if (bankingTransaction == null)
            {
                return HttpNotFound();
            }
            return View(bankingTransaction);
        }

        // POST: BankingTransactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BankingTransactionID,TransactionDispute,TransactionDate,Amount,Description, DisputeMessage, CustomerOpinion, CorrectedAmount, BankingTransactionType")] BankingTransaction bankingTransaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bankingTransaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bankingTransaction);
        }

        // GET: BankingTransactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankingTransaction bankingTransaction = db.BankingTransaction.Find(id);
            if (bankingTransaction == null)
            {
                return HttpNotFound();
            }
            return View(bankingTransaction);
        }

        // POST: BankingTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BankingTransaction bankingTransaction = db.BankingTransaction.Find(id);
            db.BankingTransaction.Remove(bankingTransaction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Portal/Checkings/Deposit/5
        // Where 5 is the CheckingID
        public ActionResult Deposit() //int? id
        {
            // TODO: ViewBag.AllAccounts 

            /*
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Checking checking = db.CheckingAccount.Find(id);
            if (checking == null)
            {
                return HttpNotFound();
            }

            */
            return View(); // TODO: need to pass the checking model object in
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deposit([Bind(Include = "BankingTransactionID,TransactionDate,Amount,Description,BankingTransactionType")] BankingTransaction bankingTransaction, Int32 CheckingID)
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

        // Get all of the customer's account 
        // id == customer's id
        public SelectList GetCheckingAccounts(int id)
        {
            // Populate a list of Accounts
            var CheckingQuery = from ca in db.CheckingAccount
                                where ca.Customer.CustomerID == id
                                select ca;

            // Create a list of accounts of execute
            List<Checking> CheckingAccounts = CheckingQuery.ToList();

            // Create a None Options 
            Checking SelectNone = new Checking() { CheckingID = 0, AccountNumber = "1000000000000", Balance = 0, Name = "None" };
            CheckingAccounts.Add(SelectNone);

            // Convert the List into a select list 
            SelectList CheckingSelectList = new SelectList(CheckingAccounts.OrderBy(a => a.CheckingID), "CheckingID", "Name");

            // Add the Accounts to the Tuple of Accounts 
            //Tuple<SelectList> Accounts = new Tuple<SelectList>(CheckingSelectList);

            return CheckingSelectList;

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
