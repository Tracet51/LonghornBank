using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LonghornBank.Models;
using System.Diagnostics;

namespace LonghornBank.Controllers
{
    public class BankingTransactionsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: BankingTransactions
        // Index Only Displays Checking 
        // Might Consider a Name Change 
        public ActionResult Index()
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
            /*

            // Get a List of Savings accounts associated with Customer ID
            List<Saving> CustomerSavings = customer.SavingAccounts;

            foreach (Saving s in CustomerSavings)
            {
                // Get the Savings Accounts ID
                Int32 inttemp = s.SavingID;

                // Select the transactions where the checking Id matches the customers checking Id
                var query = from a in db.BankingTransaction
                            from b in a.SavingsAccount
                            where b.SavingID == inttemp
                            select a;

                // Create a holding list and add to main list
                List<BankingTransaction> TempTransaction = query.ToList();
                CustomerTransactions.AddRange(TempTransaction);
            }

            // Get the IRA account associated with Customer ID
            IRA CustomerIRA = customer.IRAAccounts.FirstOrDefault();

            // Check to see if null
            if (CustomerIRA != null)
            {
                // Select the transactions where the checking Id matches the customers checking Id
                var IraQuery = from a in db.BankingTransaction
                               from b in a.IRAAccount
                               where b.IRAID == CustomerIRA.IRAID
                               select a;

                // Create a holding list and add to main list
                List<BankingTransaction> IRATransactions = IraQuery.ToList();
                CustomerTransactions.AddRange(IRATransactions);
            }
            */


            // Add the customer to the view bag
            ViewBag.Customer = customer;

            return View("Index", CustomerTransactions);
        }

        // GET: BankingTransactions/Details/5
        // id == BankingTransactionID
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
        public ActionResult Create()
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
            Tuple<SelectList, SelectList, SelectList> AllAcounts = GetAllAccounts(customer.Id);

            // Add the SelectList Tuple to the ViewBag
            ViewBag.AllAccounts = AllAcounts;

            return View();
        }

        // POST: BankingTransactions/Create
        // CheckingID = the checking account to bind to 
        // id = The Customer's Account ID
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BankingTransactionID,TransactionDate,Amount,Description,BankingTransactionType")] BankingTransaction bankingTransaction, int CheckingID, int CheckingIDTrans, int SavingID, int SavingIDTrans, int IraID, int IraIDTrans)
        {

            var CustomerQuery = from c in db.Users
                                where c.UserName == User.Identity.Name
                                select c;


            // Get the Customer 
            AppUser customer = CustomerQuery.FirstOrDefault();

            string id = customer.Id;

            if (ModelState.IsValid)
            {
                
                // Check to see if Deposit 
                if (bankingTransaction.BankingTransactionType == BankingTranactionType.Deposit)
                {
                    // Check to if Checking Account
                    if (CheckingID != 0)
                    {
                        // Find the Selected Checking Account
                        Checking SelectedChecking = db.CheckingAccount.Find(CheckingID);

                        // Create a list of checking accounts and add the one seleceted 
                        List<Checking> NewCheckingAccounts = new List<Checking> { SelectedChecking };

                        bankingTransaction.CheckingAccount = NewCheckingAccounts;

                        
                        //Adds Money to account if under amount required for approval
                        if (bankingTransaction.Amount <= 5000)
                        {
                            Decimal New_Balance = SelectedChecking.Balance + bankingTransaction.Amount;
                            SelectedChecking.Balance = New_Balance;
                        }
                        //Adds money to pending balance if transaction over 5000
                        else
                        {
                            SelectedChecking.PendingBalance = bankingTransaction.Amount;
                        }

                       

                        // Add to database
                        db.BankingTransaction.Add(bankingTransaction);
                        db.SaveChanges();

                        // Redirect 
                        return RedirectToAction("Index", "BankingTransactions", new { id = id });
                    }

                    // Check to see if Savings Account
                    if (SavingID != 0)
                    {
                        // Find the Selected Checking Account
                        Saving SelectedSaving= db.SavingsAccount.Find(SavingID);

                        // Create a list of checking accounts and add the one seleceted 
                        List<Saving> NewSavingsAccounts = new List<Saving> { SelectedSaving };

                        bankingTransaction.SavingsAccount = NewSavingsAccounts;

                        //Adds money to account if under 5000
                        if (bankingTransaction.Amount <= 5000)
                        {
                            Decimal New_Balance = SelectedSaving.Balance + bankingTransaction.Amount;
                            SelectedSaving.Balance = New_Balance;
                        }

                        //Adds to pending balance if over 5000
                        else
                        {
                            SelectedSaving.PendingBalance = bankingTransaction.Amount;
                        }



                        // Add to database
                        db.BankingTransaction.Add(bankingTransaction);
                        db.SaveChanges();

                        // Redirect 
                        return RedirectToAction("Index", "BankingTransactions", new { id = id });
                    }
                   
                }
                
                // Check to see if of Type Withdrawl
                else if (bankingTransaction.BankingTransactionType == BankingTranactionType.Withdrawl)
                {
                    // Check to if Checking Account
                    if (CheckingID != 0)
                    {
                        // Find the Selected Checking Account
                        Checking SelectedChecking = db.CheckingAccount.Find(CheckingID);

                        // Create a list of checking accounts and add the one seleceted 
                        List<Checking> NewCheckingAccounts = new List<Checking> { SelectedChecking };

                        bankingTransaction.CheckingAccount = NewCheckingAccounts;

                        //Subtracts Money to account if under amount required for approval
                        if (bankingTransaction.Amount <= SelectedChecking.Balance)
                        {
                            //TODO: Write error message for invalid withdrawl
                            
                            Decimal New_Balance = SelectedChecking.Balance - bankingTransaction.Amount;
                            SelectedChecking.Balance = New_Balance;
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

                        //Subtracts Money to account if under amount required for approval
                        if (bankingTransaction.Amount <= SelectedSaving.Balance)
                        {
                            //TODO: Write error message for invalid withdrawl
                            /*if (bankingTransaction.Amount > SelectedChecking.Balance)
                            {
                                return
                            }*/
                            Decimal New_Balance = SelectedSaving.Balance - bankingTransaction.Amount;
                            SelectedSaving.Balance = New_Balance;
                        }

                        // Add to database
                        db.BankingTransaction.Add(bankingTransaction);
                        db.SaveChanges();

                        // Redirect 
                        return RedirectToAction("Index", "BankingTransactions", new { id = id });
                    }
                }

                // If it is a transfer 
                else
                {
                    // Check to see if first account is checking
                    if (CheckingID != 0)
                    {

                        // Find the Selected Checking Account
                        Checking SelectedChecking = db.CheckingAccount.Find(CheckingID);

                        // Create a list of checking accounts and add the one seleceted 
                        List<Checking> NewCheckingAccounts = new List<Checking> { SelectedChecking };

                        // Check to see if the Transfer is to Another Checking
                        if (CheckingIDTrans != 0)
                        {
                            // Find the Selected Checking Account
                            Checking CheckingTrans = db.CheckingAccount.Find(CheckingIDTrans);

                            // Add the Transfer to the Checking List 
                            NewCheckingAccounts.Add(CheckingTrans);

                            // Create a new association of the acccounts
                            bankingTransaction.CheckingAccount = NewCheckingAccounts;

                            //Take money from checking
                            if (bankingTransaction.Amount <= SelectedChecking.Balance)
                            {
                                //TODO: Write error message for invalid withdrawl

                                Decimal New_Balance = SelectedChecking.Balance - bankingTransaction.Amount;
                                SelectedChecking.Balance = New_Balance;
                                Decimal New_Transfer_Balance = CheckingTrans.Balance + bankingTransaction.Amount;
                                CheckingTrans.Balance = New_Transfer_Balance;
                            }

                            // Add to database
                            db.BankingTransaction.Add(bankingTransaction);
                            db.SaveChanges();

                            // Redirect 
                            return RedirectToAction("Index", "BankingTransactions", new { id = id });
                        }

                        // If Transfering to Savings Account
                        else if (SavingIDTrans !=0)
                        {
                            // Find the Selected Checking Account
                            Saving SavingsTrans = db.SavingsAccount.Find(SavingIDTrans);

                            // Create a list of checking accounts and add the one seleceted 
                            List<Saving> NewSavingsAccounts = new List<Saving> { SavingsTrans };

                            // Add the Savings Account
                            bankingTransaction.SavingsAccount = NewSavingsAccounts;

                            // Add the Checking Account
                            bankingTransaction.CheckingAccount = NewCheckingAccounts;

                            //Adds money to account
                            if (bankingTransaction.Amount <= SavingsTrans.Balance)
                            {
                                //TODO: Write error message for invalid withdrawl

                                Decimal New_Balance = SavingsTrans.Balance + bankingTransaction.Amount;
                                SavingsTrans.Balance = New_Balance;
                            }

                            // Add to database
                            db.BankingTransaction.Add(bankingTransaction);
                            db.SaveChanges();

                            // Redirect 
                            return RedirectToAction("Index", "BankingTransactions", new { id = id });
                        }

                        // Redirect 
                        return RedirectToAction("Index", "BankingTransactions", new { id = id });
                    }

                    // If Transfering from a savings account 
                    else if (SavingID !=0)
                    {
                        // Find the Selected Checking Account
                        Saving SelectedSavings = db.SavingsAccount.Find(SavingID);

                        // Create a list of checking accounts and add the one seleceted 
                        List<Saving> NewSavingsAccounts = new List<Saving> { SelectedSavings };

                        // If transfering to a checking account
                        if (CheckingIDTrans != 0)
                        {
                            // Find the Selected Checking Account
                            Checking CheckingTrans = db.CheckingAccount.Find(CheckingIDTrans);

                            // Create a list of checking accounts and add the one seleceted 
                            List<Checking> NewCheckingAccounts = new List<Checking> { CheckingTrans };

                            // Add the Transfer to the Checking List 
                            NewCheckingAccounts.Add(CheckingTrans);

                            // Create a new association of the acccounts
                            bankingTransaction.CheckingAccount = NewCheckingAccounts;

                            // Associate with Savings Account 
                            bankingTransaction.SavingsAccount = NewSavingsAccounts;

                            // Add to database
                            db.BankingTransaction.Add(bankingTransaction);
                            db.SaveChanges();

                            // Redirect 
                            return RedirectToAction("Index", "BankingTransactions", new { id = id });
                        }

                        // If Transfering to a Savings Account 
                        else if (SavingIDTrans != 0)
                        {
                            // Find the Selected Checking Account
                            Saving SavingsTrans = db.SavingsAccount.Find(SavingIDTrans);

                            // Add to the Savings Account List
                            NewSavingsAccounts.Add(SavingsTrans);

                            // Associate with Savings Account 
                            bankingTransaction.SavingsAccount = NewSavingsAccounts;

                            // Add to database
                            db.BankingTransaction.Add(bankingTransaction);
                            db.SaveChanges();

                            // Redirect 
                            return RedirectToAction("Index", "BankingTransactions", new { id = id });
                        }

                    }

                }

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
                if (bankingTransaction.Amount<=5000)
                {
                    Decimal New_Balance = SelectedChecking.Balance + bankingTransaction.Amount;
                    SelectedChecking.Balance = New_Balance;  
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Repopulate the dropdown options
            ViewBag.AllAccounts = bankingTransaction.CheckingAccount.ToList();
            return View(bankingTransaction);
        }

        // Get all of the customer's account 
        // id == customer's id
        public Tuple<SelectList, SelectList, SelectList> GetAllAccounts(string id)
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

            // Grab the IRA account
            var IraQuery = from ira in db.IRAAccount
                               where ira.Customer.Id == id
                               select ira;

            // Get the IRA account
            List<IRA> IRAAccounts = IraQuery.ToList();

            // Create a None Options 
            IRA SelectNoIRA = new IRA() { IRAID = 0, AccountNumber = "1000000000000", Balance = 0, Name = "None" };
            IRAAccounts.Add(SelectNoIRA);

            // Convert the List into a select list 
            SelectList IRASelectList = new SelectList(IRAAccounts.OrderBy(a => a.IRAID), "IRAID", "Name");

            // Add the Accounts to the Tuple of Accounts 
            Tuple<SelectList, SelectList, SelectList> Accounts = new Tuple<SelectList, SelectList, SelectList>(CheckingSelectList, SavingsSelectList, IRASelectList);

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
