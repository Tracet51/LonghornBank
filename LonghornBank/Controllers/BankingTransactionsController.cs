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
            ViewBag.Ranges = AmountRange();
            ViewBag.Dates = DateRanges();
            ViewBag.DisplayedTransactionCount =CustomerTransactions.ToList().Count;
            ViewBag.TotalTransactionCount = db.BankingTransaction.ToList().Count;

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

                    //Check to see if depositing into IRRA
                    if (IraID != 0)
                    {
                        // Find the Selected Checking Account
                        IRA SelectedIra = db.IRAAccount.Find(IraID);

                        // Create a list of checking accounts and add the one seleceted 
                        List<IRA> NewIraAccounts = new List<IRA> { SelectedIra };

                        bankingTransaction.IRAAccount = NewIraAccounts;

                        //Adds money to account if under 5000
                        if (bankingTransaction.Amount + SelectedIra.RunningTotal <= 5000)
                        {
                            SelectedIra.RunningTotal = SelectedIra.RunningTotal + bankingTransaction.Amount;
                            Decimal New_Balance = SelectedIra.Balance + bankingTransaction.Amount;
                            SelectedIra.Balance = New_Balance;
                        }

                        //Adds to pending balance if over 5000
                        else
                        {
                            ViewBag.CorrectIra = "Would you like to automatically deposit " + (bankingTransaction.Amount - SelectedIra.RunningTotal) + " To make the transaction valid, or would you like to do it yourself";
                            decimal CorrectAmount = 5000 - SelectedIra.RunningTotal;
                            return RedirectToAction("IRAError", "BankingTransactions", new { CorrectAmount, bankingTransaction.BankingTransactionID, bankingTransaction.BankingTransactionType, bankingTransaction.Description, bankingTransaction.TransactionDate, customer.Id, IraID, IraIDTrans });
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
                        else
                        {
                            return View("WithdrawlError");
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

                            Decimal New_Balance = SelectedSaving.Balance - bankingTransaction.Amount;
                            SelectedSaving.Balance = New_Balance;
                        }
                        
                        //Error for withdrawling too much
                        else
                        {
                            return View("WithdrawlError");
                        }

                        // Add to database
                        db.BankingTransaction.Add(bankingTransaction);
                        db.SaveChanges();

                        // Redirect 
                        return RedirectToAction("Index", "BankingTransactions", new { id = id });
                    }

                    else if (IraID != 0)
                    {
                        DateTime Restrict1 = new DateTime(1952, 5, 5, 0, 0, 0);

                        // Find the Selected Checking Account
                        IRA SelectedIra = db.IRAAccount.Find(IraID);

                        // Create a list of checking accounts and add the one seleceted 
                        List<IRA> NewIraAccounts = new List<IRA> { SelectedIra };

                        bankingTransaction.IRAAccount = NewIraAccounts;

                        if (customer.DOB <= Restrict1)
                        {
                            if (bankingTransaction.Amount <= SelectedIra.Balance)
                            {
                                Decimal New_Balance = SelectedIra.Balance - bankingTransaction.Amount;
                                SelectedIra.Balance = New_Balance;
                            }
                            else
                            {
                                //Returns error if the withdrawl is more than bankingTransaction.Amount
                                return View("IRAError");
                            }

                        }
                        else
                        {
                            if (bankingTransaction.Amount <= SelectedIra.Balance)
                            {
                                db.BankingTransaction.Add(bankingTransaction);
                                db.SaveChanges();

                                bankingTransaction.Amount = 30;
                                bankingTransaction.BankingTransactionType = BankingTranactionType.Fee;
                            }
                        }

                        // Add to database
                        db.BankingTransaction.Add(bankingTransaction);
                        db.SaveChanges();

                        // Redirect 
                        return RedirectToAction("Index", "BankingTransactions", new { id = id });
                    }
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
                            else
                            {
                                return View("WithdrawlError");
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
                                Decimal New_Transfer_Balance = SavingsTrans.Balance - bankingTransaction.Amount;
                                SavingsTrans.Balance = New_Transfer_Balance;
                            }
                            else
                            {
                                return View("WithdrawlError");
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

                            //Adds money from savings to checking account
                            if (bankingTransaction.Amount <= SelectedSavings.Balance)
                            {
                                //Adds money to transferred account
                                Decimal New_Balance = CheckingTrans.Balance + bankingTransaction.Amount;
                                CheckingTrans.Balance = New_Balance;

                                //Takes away money from account being withdrawn from 
                                Decimal New_Transfer_Balance = SelectedSavings.Balance - bankingTransaction.Amount;
                                SelectedSavings.Balance = New_Transfer_Balance;
                            }
                            else
                            {
                                return View("WithdrawlError");
                            }


                            // Add to database
                            db.BankingTransaction.Add(bankingTransaction);
                            db.SaveChanges();

                            // Redirect 
                            return RedirectToAction("Index", "BankingTransactions", new { id = id });
                        }

                        // If Transfering to a Savings Account 
                        else if (SavingIDTrans != 0)
                        {
                            // Find the Selected saving Account
                            Saving SavingsTrans = db.SavingsAccount.Find(SavingIDTrans);

                            // Add to the Savings Account List
                            NewSavingsAccounts.Add(SavingsTrans);

                            // Associate with Savings Account 
                            bankingTransaction.SavingsAccount = NewSavingsAccounts;

                            //Adds money from savings to checking account
                            if (bankingTransaction.Amount <= SelectedSavings.Balance)
                            {
                                //Adds money to transferred account
                                Decimal New_Balance = SavingsTrans.Balance + bankingTransaction.Amount;
                                SavingsTrans.Balance = New_Balance;

                                //Takes away money from account being withdrawn from 
                                Decimal New_Transfer_Balance = SelectedSavings.Balance - bankingTransaction.Amount;
                                SelectedSavings.Balance = New_Transfer_Balance;
                            }
                            else
                            {
                                return View("WithdrawlError");
                            }

                            // Add to database
                            db.BankingTransaction.Add(bankingTransaction);
                            db.SaveChanges();

                            // Redirect 
                            return RedirectToAction("Index", "BankingTransactions", new { id = id });
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
            var CustomerQuery = from c in db.Users
                                where c.Email == User.Identity.Name
                                select c;
            AppUser Customer = CustomerQuery.FirstOrDefault();

            var CheckingQuery = from c in db.CheckingAccount
                                where c.Customer.Id == Customer.Id
                                select c;

            List<Checking> CustomerChecking = CheckingQuery.ToList();

            // Convert into a select list 
            SelectList CheckingSelectList = new SelectList(CustomerChecking, "CheckingID", "Name");

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

            // add the select list to the viewbag
            ViewBag.CheckingAccounts = CheckingSelectList;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deposit([Bind(Include = "BankingTransactionID,TransactionDate,Amount,Description")] BankingTransaction bankingTransaction, Int32 CheckingID)
        {
            // Find the selected Checking Account
            Checking SelectedChecking = db.CheckingAccount.Find(CheckingID);
            List<Checking> CheckingList = new List<Checking>();
            CheckingList.Add(SelectedChecking);

            // Associate the transaction with the checking account
            bankingTransaction.CheckingAccount = CheckingList;

            // Check to see if the model state if valid
            bankingTransaction.BankingTransactionType = BankingTranactionType.Deposit;

            db.BankingTransaction.Add(bankingTransaction);
            if (bankingTransaction.Amount<=5000)
            {
                Decimal New_Balance = SelectedChecking.Balance + bankingTransaction.Amount;
                SelectedChecking.Balance = New_Balance;  
            }
            else
            {
                Decimal PendingBalance = SelectedChecking.PendingBalance + bankingTransaction.Amount;
                SelectedChecking.PendingBalance = PendingBalance;
            }
            db.Entry(SelectedChecking).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

            // Repopulate the dropdown options
            var CheckingQuery = from c in db.CheckingAccount
                                where c.Customer.Id == SelectedChecking.Customer.Id
                                select c;

            List<Checking> CustomerChecking = CheckingQuery.ToList();

            // Convert into a select list 
            SelectList CheckingSelectList = new SelectList(CustomerChecking, "CheckingID", "Name");
            ViewBag.CheckingAccounts = CheckingSelectList;
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

        public ActionResult IRAError()
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IRAError([Bind(Include = "BankingTransactionID,TransactionDate,Amount,Description,BankingTransactionType")]BankingTransaction bankingTransaction, Decimal CorrectAmount, Int32 BankingTransactionID, String Description, DateTime TransactionDate, string ID, int IraID, int IraIDTrans, String submit)
        {
            var CustomerQuery = from c in db.Users
                                where c.UserName == User.Identity.Name
                                select c;


            // Get the Customer 
            AppUser customer = db.Users.Find(ID);
            
            if(customer == null)
            {
                return RedirectToAction("Portal", "Home");
            }
                bankingTransaction.Description = Description;
                bankingTransaction.TransactionDate = TransactionDate;
                bankingTransaction.BankingTransactionID = BankingTransactionID;
                bankingTransaction.BankingTransactionType = BankingTranactionType.Deposit;
                // Check to see if Deposit 
                if (bankingTransaction.BankingTransactionType == BankingTranactionType.Deposit)
                {
                    if (IraID != 0)
                    {
                        // Find the Selected Checking Account
                        IRA SelectedIra = db.IRAAccount.Find(IraID);

                        // Create a list of checking accounts and add the one seleceted 
                        List<IRA> NewIraAccounts = new List<IRA> { SelectedIra };

                        bankingTransaction.IRAAccount = NewIraAccounts;

                        switch (submit)
                        {
                            case "Automatic":
                                Decimal New_Balance = SelectedIra.Balance + CorrectAmount;
                                bankingTransaction.Amount = New_Balance;
                                SelectedIra.RunningTotal = New_Balance;
                                SelectedIra.Balance = 0 + New_Balance;
                                db.BankingTransaction.Add(bankingTransaction);
                                db.SaveChanges();
                                break;

                            case "User":
                                if (bankingTransaction.Amount + SelectedIra.RunningTotal <= 5000)
                                {
                                    SelectedIra.RunningTotal = SelectedIra.RunningTotal + bankingTransaction.Amount;
                                    Decimal New_Balance2 = SelectedIra.Balance + bankingTransaction.Amount;
                                    SelectedIra.RunningTotal = 0 + New_Balance2;
                                    SelectedIra.Balance = New_Balance2;
                                    db.BankingTransaction.Add(bankingTransaction);
                                    db.SaveChanges();
                                }

                                else
                                {
                                    return RedirectToAction("IRAError", "BankingTransactions", new { CorrectAmount, bankingTransaction.BankingTransactionID, bankingTransaction.BankingTransactionType, bankingTransaction.Description, bankingTransaction.TransactionDate, IraID, IraIDTrans });
                                }
                                break;
                        }
                    }

                }
            // Redirect 
            return RedirectToAction("Index", "BankingTransactions", new { id = customer.Id });
        }


        public static DateTime Today { get;}

        //Detailed Search function
        public ActionResult SearchResults(String SearchDescription, BankingTranactionType SelectedType, String SearchAmountBegin, String SearchAmountEnd, Int32 SearchAmountRange, String SearchTransactionNumber, String BeginSearchDate, String EndSearchDate, Int32 DateRange, SortingOption SortType)
        {

            var query = from c in db.BankingTransaction
                        select c;

            if (SearchDescription != null)
            {
                query = query.Where(c => c.Description.Contains(SearchDescription));
            }

            if (SelectedType != BankingTranactionType.None)
            {
                //Search for transactions with type deposit
                if (SelectedType == BankingTranactionType.Deposit)
                {
                                        
                  query = query.Where(c => c.BankingTransactionType == BankingTranactionType.Deposit);
                }

                //Search for transactions with type withdrawl
                if (SelectedType == BankingTranactionType.Withdrawl)
                {
                    query = query.Where(c => c.BankingTransactionType == BankingTranactionType.Withdrawl);
                }

                //Search for transactions with type transfer
                if (SelectedType == BankingTranactionType.Transfer)
                {
                    query = query.Where(c => c.BankingTransactionType == BankingTranactionType.Transfer);
                }

                //Search for transactions with type fees
                if (SelectedType == BankingTranactionType.Fee)
                {
                    query = query.Where(c => c.BankingTransactionType == BankingTranactionType.Fee);
                }
                
                
            }

            // Convert them to a string
            if (!String.IsNullOrEmpty(SearchAmountBegin) && !String.IsNullOrEmpty(SearchAmountEnd) && (SearchAmountRange == 0 || SearchAmountRange == 5))
            {
                Decimal AmountBegin = Convert.ToDecimal(SearchAmountBegin);
                Decimal AmountEnd = Convert.ToDecimal(SearchAmountEnd);

                if (AmountBegin >= 0 && AmountEnd > AmountBegin && (SearchAmountRange == 0 || SearchAmountRange ==5))
                {
                    query = query.Where(c => c.Amount >= AmountBegin && c.Amount <= AmountEnd);
                }
            }
            
            if (SearchAmountRange != 0 && SearchAmountRange!=5)
            {
                //For 0 to 100
                if (SearchAmountRange == 1)
                {
                    query = query.Where(c => c.Amount >= 0 && c.Amount <= 100);
                }

                //For 100 to 200
                if (SearchAmountRange == 2)
                {
                    query = query.Where(c => c.Amount >=100 && c.Amount <= 200);
                }

                //For 200 to 300
                if (SearchAmountRange == 3)
                {
                    query = query.Where(c => c.Amount >= 200 && c.Amount <= 300);
                }

                //For 300+
                if (SearchAmountRange == 4)
                {
                    query = query.Where(c => c.Amount >= 300);
                }
            }

            if (!String.IsNullOrEmpty(BeginSearchDate) && !String.IsNullOrEmpty(EndSearchDate) && (DateRange == 0 || DateRange == 4))
            {
                // Convert to Datetime 
                DateTime BeginDate = Convert.ToDateTime(BeginSearchDate);
                DateTime EndDate = Convert.ToDateTime(EndSearchDate);

                if (BeginDate < DateTime.Today && EndDate > BeginDate && (DateRange == 0 || DateRange == 4))
                {
                    query = query.Where(c => c.TransactionDate >= BeginDate && c.TransactionDate <= EndDate);
                }

            }

            //0 should indicate searching for all dates
            if (DateRange != 0)
            {
                //Last 15 days
                if (DateRange == 1)
                {
                    query = query.Where(c => c.TransactionDate >= Today.AddDays(-15));
                }

                //Last 30 days
                if (DateRange == 2)
                {
                    query = query.Where(c => c.TransactionDate <= Today.AddDays(-15) && c.TransactionDate >= Today.AddDays(-30));
                }
                //Last 60 days
                if (DateRange == 3)
                {
                    query = query.Where(c => c.TransactionDate <= Today.AddDays(-30) && c.TransactionDate >= Today.AddDays(-60));
                }

            }

            if (SearchTransactionNumber != null)
            {
                query = query.Where(c => c.Description.Contains(SearchTransactionNumber));
            }

            //Order Trans ID ascending
            if (SortType != SortingOption.TransIDAsc)
            {
                //query = query.OrderBy(c => c.BankingTransactionID);
                //Order Trans ID descending
                if (SortType == SortingOption.TransIDDec)
                {

                    query = query.OrderByDescending(c => c.BankingTransactionID);
                }

                //Order by type ascending
                if (SortType == SortingOption.TransTypeAsc)
                {
                    query = query.OrderBy(c => c.BankingTransactionType);
                }

                //Order by type descending
                if (SortType == SortingOption.TransTypeDec)
                {
                    query = query.OrderByDescending(c => c.BankingTransactionType);
                }

                //Order by Description Ascending
                if (SortType == SortingOption.TransDescriptionAsc)
                {
                    query = query.OrderBy(c => c.Description);
                }

                //Order by description descending
                if (SortType == SortingOption.TransDescriptionDec)
                {
                    query = query.OrderByDescending(c => c.Description);
                }
                //Order by amount ascending
                if (SortType == SortingOption.TransAmountAsc)
                {
                    query = query.OrderBy(c => c.Amount);
                }

                //Order by amount descending
                if (SortType == SortingOption.TransAmountDec)
                {
                    query = query.OrderByDescending(c => c.Amount);
                }

                if (SortType == SortingOption.TransDateAsc)
                {
                    query = query.OrderBy(C => C.TransactionDate);
                }

                if (SortType == SortingOption.TransDateDec)
                {

                    query = query.OrderByDescending(c => c.TransactionDate);
                }

            }


            ViewBag.DisplayedTransactionCount = query.ToList().Count;
            ViewBag.TotalTransactionCount = db.BankingTransaction.ToList().Count;
            ViewBag.Ranges = AmountRange();
            ViewBag.Dates = DateRanges();
            List<BankingTransaction> SelectedTransactions = query.ToList();
            return View("Index", SelectedTransactions);
        }
        

        public SelectList GetAllBankingTypesList()
        {
            List<BankingTranactionType> BBT = Enum.GetValues(typeof(BankingTranactionType)).Cast<BankingTranactionType>().ToList();
            SelectList BBTSelectList = new SelectList(BBT, new BankingTranactionType());
            ViewBag.AllBankingTypes = BBTSelectList;
            return BBTSelectList;
        }

        public SelectList AmountRange()
        {
            // Create a list of ranges
            List<Ranges> RangesList = new List<Ranges>();

            Ranges None = new Ranges { Name = "None", RangeID = 0 };
            Ranges _100 = new Ranges { Name = "$0-100", RangeID = 1 };
            Ranges _100_200 = new Ranges { Name = "$100-200", RangeID = 2 };
            Ranges _200_300 = new Ranges { Name = "$200-300", RangeID = 3 };
            Ranges _300 = new Ranges { Name = "$300+", RangeID = 4 };
            Ranges Custom = new Ranges { Name = "Custom", RangeID = 5 };

            // Add to the list
            RangesList.Add(None);
            RangesList.Add(_100);
            RangesList.Add(_100_200);
            RangesList.Add(_200_300);
            RangesList.Add(_300);
            RangesList.Add(Custom);
            

            // Convert to Select List

            SelectList RangesSelect = new SelectList(RangesList, "RangeID", "Name");

            return RangesSelect;
        }

        public SelectList DateRanges()
        {
            // Create a list for Dates
            List<RangesDate> RangeDates = new List<RangesDate>();

            RangesDate All = new RangesDate { Name = "All Available", RangeID = 0 };
            RangesDate Last15 = new RangesDate { Name = "Last 15 Days", RangeID = 1 };
            RangesDate Last30 = new RangesDate { Name = "Last 30", RangeID = 2 };
            RangesDate Last60 = new RangesDate { Name = "Last 60", RangeID = 3 };
            RangesDate Custom = new RangesDate { Name = "Custom", RangeID = 4 };

            // add to the list 
            RangeDates.Add(All);
            RangeDates.Add(Last15);
            RangeDates.Add(Last30);
            RangeDates.Add(Last60);
            RangeDates.Add(Custom);

            // create a select list 
            SelectList DateSelect = new SelectList(RangeDates, "RangeID", "Name");

            return DateSelect;
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
