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
    public class PayeesController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Payees
        public ActionResult Index()
        {
            return View(db.Payees.ToList());
        }

        // GET: Payees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payee payee = db.Payees.Find(id);
            if (payee == null)
            {
                return HttpNotFound();
            }
            return View(payee);
        }

        // GET: Payees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Payees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PayeeID,Name,StreetAddress,City,State,Zip,PhoneNumber,PayeeType")] Payee payee)
        { 

            if (ModelState.IsValid)
            {
                db.Payees.Add(payee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(payee);
        }

        // GET: Payees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payee payee = db.Payees.Find(id);
            if (payee == null)
            {
                return HttpNotFound();
            }
            return View(payee);
        }

        // POST: Payees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PayeeID,Name,StreetAddress,City,State,Zip,PhoneNumber,PayeeType")] Payee payee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(payee);
        }

        // GET: Payees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payee payee = db.Payees.Find(id);
            if (payee == null)
            {
                return HttpNotFound();
            }
            return View(payee);
        }

        // POST: Payees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payee payee = db.Payees.Find(id);
            db.Payees.Remove(payee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


       public ActionResult PayBillsPage(int? id)
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

            // Create list and execute the query 
            List<Payee> CustomerPayee = customer.PayeeAccounts;

            // Add the Customer to ViewBag to Access information 
            ViewBag.CustomerAccountPayee = customer;

            return View("PayBillsPage", CustomerPayee);
        }

        public ActionResult EditOwnPayee(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payee payee = db.Payees.Find(id);
            if (payee == null)
            {
                return HttpNotFound();
            }
            return View(payee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOwnPayee([Bind(Include = "PayeeID,Name,StreetAddress,City,State,Zip,PhoneNumber,PayeeType")] Payee payee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PayBillsPage");
            }
            return View(payee);
        }

        public ActionResult PayPayee()
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

            // Get all of the accounts
            Tuple<SelectList, SelectList> AllCAndSAccounts = GetAllCheckingAndSavings(customer.Id);

            // Add the SelectList Tuple to the ViewBag
            ViewBag.AllAccounts = AllCAndSAccounts;

            return View();
        }

        // POST: BankingTransactions/Create
        // CheckingID = the checking account to bind to 
        // id = The Customer's Account ID
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PayPayee([Bind(Include = "BankingTransactionID,TransactionDate,Amount,Description,BankingTransactionType")] BankingTransaction bankingTransaction, int CheckingID, int CheckingIDTrans, int SavingID, int SavingIDTrans, int IraID, int IraIDTrans)
        {
            var CustomerQuery = from c in db.Users
                                where c.UserName == User.Identity.Name
                                select c;


            // Get the Customer 
            AppUser customer = CustomerQuery.FirstOrDefault();

            string id = customer.Id;

            if (ModelState.IsValid)
            {      // Check to if Checking Account
                    if (CheckingID != 0)
                    {
                        // Find the Selected Checking Account
                        Checking SelectedChecking = db.CheckingAccount.Find(CheckingID);

                        // Create a list of checking accounts and add the one seleceted 
                        List<Checking> NewCheckingAccounts = new List<Checking> { SelectedChecking };

                        bankingTransaction.CheckingAccount = NewCheckingAccounts;
                        if (SelectedChecking.Balance >= 0)
                        {
                        //Subtracts Money to account if under amount required for approval
                          if (bankingTransaction.Amount <= SelectedChecking.Balance)
                          {
                            Decimal New_Balance = SelectedChecking.Balance - bankingTransaction.Amount;
                            SelectedChecking.Balance = New_Balance;
                          }
                          if (bankingTransaction.Amount <= SelectedChecking.Balance+50)
                          {
                            bankingTransaction.Amount = 30;
                            bankingTransaction.BankingTransactionType = BankingTranactionType.Fee;

                            db.BankingTransaction.Add(bankingTransaction);
                            db.SaveChanges();

                            SelectedChecking.Balance = SelectedChecking.Balance + 50;
                            Decimal New_Balance = SelectedChecking.Balance - bankingTransaction.Amount;
                            SelectedChecking.Balance = New_Balance;
                          }
                          if(bankingTransaction.Amount > SelectedChecking.Balance+50)
                          {

                          }
                    }



                        // Add to database
                        db.BankingTransaction.Add(bankingTransaction);
                        db.SaveChanges();

                        // Redirect 
                        return RedirectToAction("Index", "BankingTransactions", new { id = id });
                    }

                    // Check to see if Savings Account
                    else if (SavingID != 0)
                    {
                        // Find the Selected Checking Account
                        Saving SelectedSaving = db.SavingsAccount.Find(SavingID);

                        // Create a list of checking accounts and add the one seleceted 
                        List<Saving> NewSavingsAccounts = new List<Saving> { SelectedSaving };

                        bankingTransaction.SavingsAccount = NewSavingsAccounts;
                        if (SelectedSaving.Balance >= 0)
                        {
                        //Subtracts Money to account if under amount required for approval
                          if (bankingTransaction.Amount <= SelectedSaving.Balance)
                          {
                            Decimal New_Balance = SelectedSaving.Balance - bankingTransaction.Amount;
                            SelectedSaving.Balance = New_Balance;
                          }
                          if (bankingTransaction.Amount <= SelectedSaving.Balance +50)
                          {
                            bankingTransaction.Amount = 30;
                            bankingTransaction.BankingTransactionType = BankingTranactionType.Fee;

                            db.BankingTransaction.Add(bankingTransaction);
                            db.SaveChanges();

                            SelectedSaving.Balance = SelectedSaving.Balance + 50;
                            Decimal New_Balance = SelectedSaving.Balance - bankingTransaction.Amount;
                            SelectedSaving.Balance = New_Balance;
                          }
                        if (bankingTransaction.Amount > SelectedSaving.Balance+50)
                          {

                          }
                         }

                        // Add to database
                        db.BankingTransaction.Add(bankingTransaction);
                        db.SaveChanges();

                    // Redirect 
                    return RedirectToAction("PayBillsPage", new { id = id });
                    }
                    }
            return View("PayBillsPage");
        }

        public Tuple<SelectList, SelectList> GetAllCheckingAndSavings(string id)
        {
            // Populate a list of Checking Accounts
            var CheckingQuery = from ca in db.CheckingAccount
                                where ca.Customer.Id == id
                                select ca;

            // Create a list of accounts of execute
            List<Checking> CheckingAccounts = CheckingQuery.ToList();

            // Create a None Options 
            Checking SelectNone = new Checking() { CheckingID = 0, AccountNumber = "1000000000000", Balance = 0, Name = "None" };
            CheckingAccounts.Add(SelectNone);

            // Convert the List into a select list 
            SelectList CheckingSelectList = new SelectList(CheckingAccounts.OrderBy(a => a.CheckingID), "CheckingID", "Name");

            // Populate a list of Savings Accounts
            var SavingsQuery = from sa in db.SavingsAccount
                               where sa.Customer.Id == id
                               select sa;

            // Create a list of accounts of execute
            List<Saving> SavingsAccounts = SavingsQuery.ToList();

            // Create a None Options 
            Saving SelectNoneSavings = new Saving() { SavingID = 0, AccountNumber = "1000000000000", Balance = 0, Name = "None" };
            SavingsAccounts.Add(SelectNoneSavings);

            // Convert the List into a select list 
            SelectList SavingsSelectList = new SelectList(SavingsAccounts.OrderBy(a => a.SavingID), "SavingID", "Name");

            // Add the Accounts to the Tuple of Accounts 
            Tuple<SelectList, SelectList> Accounts = new Tuple<SelectList, SelectList>(CheckingSelectList, SavingsSelectList);

            return Accounts;

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
